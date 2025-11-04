using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentation
{

    [ApiController]

    [Route("api/[controller]")]

    public class BuggyController :ControllerBase
    {
        //baseurl/api/Buggy/notfound
        [HttpGet("notfound")]

        public IActionResult GetNotFoundRequest()
        {

            return NotFound(); //404 

        }



        //baseurl/api/Buggy/servererror
        [HttpGet("servererror")]

        public IActionResult GetServerErrorRequest()
        {


            //error in server  in backend 
            //server side error 


            throw new Exception();   //500
            return Ok();

        }



        //baseurl/api/Buggy/badrequest
        [HttpGet("badrequest")]

        public IActionResult GetBadRequest()
        {



            return BadRequest(); // 400

        }

        //baseurl/api/Buggy/badrequest/ahmed
        [HttpGet(template: "badrequest/{id}")]

        public IActionResult GetBadRequest(int id) //validation error 
        {




            return BadRequest(); // 400

        }


        //baseurl/api/Buggy/unauthorized
        [HttpGet("unauthorized")]

        public IActionResult GetUnauthorizedRequest()
        {



            return Unauthorized();  // 401 


        }

    }
}
