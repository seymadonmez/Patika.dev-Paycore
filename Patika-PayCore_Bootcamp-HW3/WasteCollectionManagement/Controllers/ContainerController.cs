using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteCollectionManagement.Context;
using WasteCollectionManagement.Models;

namespace WasteCollectionManagement.Controllers
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
                Log.Error(ex, "Container Insert Error");
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
                Log.Error(ex, "Book Insert Error");
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
                Log.Error(ex, "Book Insert Error");
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

    }
}
