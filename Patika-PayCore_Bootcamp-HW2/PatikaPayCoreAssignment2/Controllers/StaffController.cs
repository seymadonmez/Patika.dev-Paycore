using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatikaPayCoreAssignment2.Entity;
using PatikaPayCoreAssignment2.FluentValidation;
using System.Linq;

namespace PatikaPayCoreAssignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        
        private IValidator<Staff> _validator;

        private static List<Staff> _staffList;

        public StaffController(IValidator<Staff> validator)
        {
            _validator = validator;
            _staffList = new List<Staff>();

            Staff staff1= new Staff
            {
                Id = 1,
                Name = "Şeyma",
                LastName = "Dönmez",
                Email = "seymadonmezz1@gmail.com",
                PhoneNumber = "+905554443322",
                DateOfBirth = new DateTime(1994, 09, 21),
                Salary = 5000
            };

            Staff staff2 =new Staff
            {
                Id = 2,
                Name = "Eda",
                LastName = "Dönmez",
                Email = "eda@gmail.com",
                PhoneNumber = "+905554443322",
                DateOfBirth = new DateTime(1998, 05, 13),
                Salary = 6000
            };
            _staffList.Add(staff1);
            _staffList.Add(staff2);
            _staffList.Add(new Staff { Id = 3, Name = "Aylin", LastName = "Sezgin", DateOfBirth = new DateTime(1999, 08, 19), Email = "aylin@aylin.com", PhoneNumber = "+905554443322", Salary = 5500 });
        }


        // Staff listesindeki tüm elemanları getiren metod
        [HttpGet("GetAll")]
        public List<Staff> GetAll()
        {
            var result = _staffList.OrderBy(x => x.Id).ToList<Staff>();
            return result;
        }


        // Staff türünde yeni bir eleman eklemeye yarayan metod
        [HttpPost("AddStaff")]
        public  IActionResult AddStaff([FromBody] Staff request)
        {
            ValidationResult result = _validator.Validate(request);

            if (result.IsValid)
            {
                _staffList.Add(request);
                return Ok();
            }

            else return BadRequest();
        }

        // Girilen id değerine karşılık olan staff nesnesini getiren metod
        [HttpGet("GetById/{id}")]
        public ActionResult<Staff> GetById(int id)
        {
            var staff = _staffList.Where(x => x.Id == id).SingleOrDefault();

            if (id == null)
                return BadRequest();

            if (staff == null)
                return NotFound();
            
            return staff;
        }

        //Girilen id değerine karşılık olan staff nesnesini güncelleyen metod
        [HttpPut("UpdateStaff")]
        public IActionResult UpdateStaff([FromBody] Staff updatedStaff, int id)
        {
            var staff = _staffList.Where(x => x.Id == id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                staff.Name = updatedStaff.Name != default ? updatedStaff.Name : staff.Name;
                staff.LastName = updatedStaff.LastName != default ? updatedStaff.LastName : staff.LastName;
                staff.Email = updatedStaff.Email != default ? updatedStaff.Email : staff.Email;
                staff.DateOfBirth = updatedStaff.DateOfBirth != default ? updatedStaff.DateOfBirth : staff.DateOfBirth;
                staff.Salary = updatedStaff.Salary != default ? updatedStaff.Salary : staff.Salary;

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // Girilen id değerine karşılık olan staff nesnesini silen metod
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete([FromBody] int id)
        {
            var staff = _staffList.SingleOrDefault(x=>x.Id==id);

            if(staff is null)
                return NotFound();
            
            _staffList.Remove(staff);
            return Ok();
            
        }
    }
}

