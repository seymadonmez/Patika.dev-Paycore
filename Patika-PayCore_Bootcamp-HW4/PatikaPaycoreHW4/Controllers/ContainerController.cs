using Microsoft.AspNetCore.Mvc;
using PatikaPaycoreHW4.Context;
using PatikaPaycoreHW4.Models;
//using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatikaPaycoreHW4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IMapperSession<Container> _session;
        public ContainerController(IMapperSession<Container> session)
        {
            _session = session;
        }

        //Sistemdeki tüm containerları listeleyen metot
        [HttpGet("GetAll")]
        public List<Container> GetAll()
        {
            List<Container> result = _session.GetAll();
            return result;
        }


        [HttpGet("{id}")]
        public Container Get(int id)
        {
            Container result = _session.GetAll().Where(x => x.ContainerId == id).FirstOrDefault();
            return result;
        }

        // Sisteme container ekleyen metot
        [HttpPost]
        public void AddContainer([FromBody] Container Container)
        {
            try
            {
                _session.BeginTransaction();
                _session.Save(Container);
                _session.Commit();
            }
            catch (Exception ex)
            {
                _session.Rollback();
                //Log.Error(ex, "Container Insert Error");
            }
            finally
            {
                _session.CloseTransaction();
            }
        }

        //Sistemdeki container'ı güncelleyen metot
        [HttpPut]
        public ActionResult<Container> Update([FromBody] Container request)
        {

            Container container = _session.GetAll().Where(x => x.ContainerId == request.ContainerId).FirstOrDefault();
            if (container == null)
            {
                return NotFound();
            }

            try
            {
                _session.BeginTransaction();

                container.ContainerName = request.ContainerName;
                container.Latitude = request.Latitude;
                container.Longitude = request.Longitude;
                container.VehicleId = request.VehicleId;

                _session.Update(container);

                _session.Commit();
            }
            catch (Exception ex)
            {
                _session.Rollback();
                //Log.Error(ex, "Book Insert Error");
            }
            finally
            {
                _session.CloseTransaction();
            }


            return Ok();
        }

        // Verilen Container Id değerine karşılık gelen container'ı silen metot
        [HttpDelete("{id}")]
        public ActionResult<Container> Delete(int id)
        {
            Container container = _session.GetAll().Where(c => c.ContainerId == id).FirstOrDefault();
            if (container == null)
            {
                return NotFound();
            }

            try
            {
                _session.BeginTransaction();
                _session.Delete((int)container.ContainerId);
                _session.Commit();
            }
            catch (Exception ex)
            {
                _session.Rollback();
                //Log.Error(ex, "Book Insert Error");
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok();
        }

        // Verilen Vehicle Id değerine göre o araca ait container listesi getiren metot
        [HttpGet("GetByVehicleId")]
        public List<Container> GetByVehicleId(long id)
        {
            return _session.GetAll().Where(c => c.VehicleId == id).ToList();
        }
        // Araca ait ccontainerları eşit eleman olacak şekilde n kümeye ayırıp response'ta kümeleri veren metot
        [HttpGet("GetClusteredContainerList")]
        public IActionResult GetClusteredContainerList(long vehicleId,int n)
        {
            var vehicleIdContainerList = _session.Entities.Where(v => v.VehicleId == vehicleId).ToList();

            var containerList = vehicleIdContainerList.Select(
                    (container, index) => new
                    {
                        ClusterIndex = index % n ,
                        Value = container
                    }).GroupBy(c => c.ClusterIndex, c => c.Value) 
                    .ToList();
            

         return Ok(containerList);
        }

        [HttpGet("GetClusteredContainerListByKMeans")]
        public IActionResult GetClusteredContainerListByKMeans(long vehicleId, int n)
        {
            var vehicleIdContainerList = _session.Entities.Where(v => v.VehicleId == vehicleId).Select(c => Tuple.Create(c.Latitude, c.Longitude)).ToList();

            var containerValuesArray = vehicleIdContainerList.Select(n => new double[] { n.Item1, n.Item2 }) // her bir satır için double dizi dön
                .ToArray(); // double dizilerinden bir dizi oluştur


            var random = new Random();
            // Her satırı rastgele bir kümeye ata


            var resultSet = Enumerable
                                        .Range(0, containerValuesArray.Length) 
                                        .Select(index => (AssignedCluster: random.Next(0, n), 
                                                      Values: containerValuesArray[index]))
                                        .ToList();

            var size = containerValuesArray[0].Length;
            var limit = 10000;
            var isChanged = true;
            while (--limit > 0)
            {
                // kümelerin merkez noktalarını hesapla
                var centerPoints = Enumerable.Range(0, n) //küme sayısı kadar sayı üretilir
                                                .AsParallel() // ardından gelecek döngünün multi thread çalışmasını sağlar. 
                                                .Select  // Select metodunda ise her küme için merkez nokta hesaplanır.
                                                (clusterNumber =>
                                                (cluster: clusterNumber,
                                                centerPoint: Enumerable.Range(0, size)
                                                                                    .Select(eksen => resultSet.Where(s => s.AssignedCluster == clusterNumber)
                                                                                    .Average(s => s.Values[eksen]))
                                                                                    .ToArray())
                                                        ).ToArray();
                // Sonuç kümesini merkeze en yakın ile güncelle
                isChanged = false;
                //for (int i = 0; i < resultSet.Count; i++)
                Parallel.For(0, resultSet.Count, i =>
                {
                    var clusterRaw = resultSet[i]; //satır değişkeni mevcut veriyi tutmakta görevinde. Peşinden gelen oldCluster ise bu satır için atanan bir önceki kümeyi tutuyor.
                    var oldCluster = clusterRaw.AssignedCluster;
                   
                    //Yeni atanacak kümenin tespitinde her bir merkez noktayı tek tek dönüyoruz ve clusterRaw değerine olan uzaklığını ölçüyoruz. Sonra bu ölçümleri sıralayıp en düşüğünün küme numarasını dönüyoruz.
                    var newCluster = centerPoints.Select(n => (clusterNumber: n.cluster,
                                                                    Distance: CalculateDistance(clusterRaw.Values, n.centerPoint)))
                                         .OrderBy(x => x.Distance)
                                         .First()
                                         .clusterNumber;
                    //Yeni atanacak küme, eskisi ile aynı değilse o zaman sonuç kümemizde ilgili satırı güncelliyoruz ve güncellenen en az 1 veri olduğu için isChanged değerini true konumuna çekiyoruz
                    if (newCluster != oldCluster)
                    {
                        resultSet[i] = (AssignedCluster: newCluster, Values: clusterRaw.Values);
                        isChanged = true;
                    }
                });

                if (!isChanged)
                {
                    break;
                }
            } // while

            return Ok(resultSet.Select(c=> (Cluster: c.AssignedCluster,  Values:c.Values.ToList()).Values.ToList()));
        }


        private double CalculateDistance(double[] latitude, double[] longitude)
        {
            var squaredDistance = latitude
                                    .Zip(longitude, //Zip metodu iki adet dizinin sırayla elemanlarını dönen bir Linq metodudur.
                                        (n1, n2) => Math.Pow(n1 - n2, 2)).Sum();
            return Math.Sqrt(squaredDistance);
        }

    }
}
