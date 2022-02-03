using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;

        public CitiesController(IAppRepository appRepository,IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult GetCities() // Şehirleri getir ve foto url getir.
        {
            var cities = _appRepository.GetCities();
            var citiesToReturn = _mapper.Map<List<CityForListDto>>(cities);
            return Ok(citiesToReturn);
        }


        [HttpPost]
        [Route("add")]
        public ActionResult Add([FromBody]City city) // Şehir Kaydet
        {
            _appRepository.Add(city);
            _appRepository.SaveAll();
            return Ok(city);
        }


        [HttpGet]
        [Route("Detail")]
        public ActionResult GetCitiesById(int Id) //Şehir Detay
        {
            var city = _appRepository.GetCityById(Id);
            var cityToReturn = _mapper.Map<CityForDetailDto>(city);
            return Ok(cityToReturn);
        }

        [HttpGet]
        [Route("Photos")]
        public ActionResult GetPhotosByCity(int cityId) // Şehir Id göre foto getirme.
        {
            var photos = _appRepository.GetPhotosByCity(cityId);
            return Ok(photos);
        }






    }
}
