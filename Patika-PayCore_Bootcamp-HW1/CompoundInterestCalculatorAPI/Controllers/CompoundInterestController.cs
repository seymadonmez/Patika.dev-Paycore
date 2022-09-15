using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using CompoundInterestCalculatorAPI.Entity;

namespace CompoundInterestCalculatorAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CompoundInterestController : ControllerBase
    {
        public CompoundInterestController()
        {

        }

        [HttpGet]
        [Route("GetCompoundInterest")]
        public ActionResult GetCompoundInterest([FromQuery] double capital, double interestRate, int time)
        {
            //Girilen değerlerin geçerliliği kontrol edilir.
            if ((interestRate < 0) || (capital < 0) || (time < 0))
            {
                return BadRequest("Please enter a valid value");
            }

            //Girilen parametrelere göre bileşik faiz getirisi hesaplanır.
            double calculatedAmount = capital * Math.Pow(1 + (interestRate / 100), time);

            
            InterestResponse result = new InterestResponse();

            //Vade sonunda elde edilen toplam bakiye
            result.TotalBalance = calculatedAmount;
            //Vade sonunda kazanılan faiz miktarı
            result.InterestAmount = calculatedAmount-capital;
            //Faiz oranı
            result.InterestRate = interestRate;

            //Vade sonunda toplam bakiye, kazanılan faiz tutarı ve faiz oranı response'da gösterilir.
            return Ok(result);
        }


        [HttpPost]
        [Route("GetCompoundInterest")]
        public ActionResult GetCompoundInterestPost([FromBody] InterestRequest request)
        {
            //Girilen değerlerin geçerliliği kontrol edilir.
            if (request==null)
            {
                return BadRequest("Please enter a valid value");
            }

            //Girilen parametrelere göre bileşik faiz getirisi hesaplanır.
            var calculatedAmount = request.Capital * Math.Pow(1 + (request.InterestRate / 100), request.DueTime);


            InterestResponse response = new InterestResponse();

            //Vade sonunda elde edilen toplam bakiye
            response.TotalBalance = calculatedAmount;
            //Vade sonunda kazanılan faiz miktarı
            response.InterestAmount = calculatedAmount - request.Capital;
            //Faiz oranı
            response.InterestRate = request.InterestRate;

            //Vade sonunda toplam bakiye, kazanılan faiz tutarı ve faiz oranı response'da gösterilir.
            return Ok(response);
        }


    }


}
