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
    public class VehicleController : ControllerBase
    {
        private readonly IMapperSession<Vehicle> _session;
        public VehicleController(IMapperSession<Vehicle> session)
        {
            _session = session;
        }

        //Sistemdeki tüm araçları listeleyen metot
        [HttpGet("GetAll")]
        public List<Vehicle> GetAll()
        {
            List<Vehicle> result = _session.GetAll();
            return result;
        }

        // Verilen araç numarasına göre araç bilgisi getiren metot
        [HttpGet("{id}")]
        public Vehicle GetById(int id)
        {
            Vehicle result = _session.GetById(id);
            return result;
        }

        // Sisteme yeni araç ekleyen metot
        [HttpPost]
        public void AddVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                _session.BeginTransaction();
                _session.Save(vehicle);
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
        }

        // Sistemdeki araç bilgisini güncelleyen metot
        [HttpPut]
        public ActionResult<Vehicle> Update([FromBody] Vehicle request)
        {

            Vehicle vehicle = _session.GetAll().Where(x => x.VehicleId == request.VehicleId).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                _session.BeginTransaction();

                vehicle.VehicleName = request.VehicleName;
                vehicle.VehiclePlate = request.VehiclePlate;

                _session.Update(vehicle);

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

        // Verilen araç numarasına göre aracı silen metot
        [HttpDelete("{id}")]
        public ActionResult<Vehicle> Delete(int id)
        {
            Vehicle vehicle = _session.GetAll().Where(v => v.VehicleId == id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                _session.BeginTransaction();
                _session.Delete((int)vehicle.VehicleId);
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


    }
}
