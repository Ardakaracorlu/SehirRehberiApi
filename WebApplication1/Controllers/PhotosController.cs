﻿using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IAppRepository _appRepository;
        private Cloudinary _cloudinary;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public PhotosController(IAppRepository appRepository,IMapper mapper,IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _appRepository = appRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }


        [HttpPost("cities/{CityId}/photos")]

        public ActionResult AddPhotoForCity(int CityId, [FromBody]PhotoForCreatinDto photoForCreatinDto)
        {
            var city = _appRepository.GetCityById(CityId);

            if (city == null)
            {
                return BadRequest("Could not find the city");
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId! == city.UserId) // Kullanıcı Farklı bir adamın şehrine resim eklemesin diye 
            {
                return Unauthorized();
            }

            var file = photoForCreatinDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length >0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreatinDto.Url = uploadResult.Uri.ToString();
            photoForCreatinDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreatinDto);
            photo.City = city;

            if (!city.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }
            city.Photos.Add(photo);
            if (_appRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");

        }

        [HttpGet("{id}",Name ="GetPhoto")]
        public ActionResult GetPhoto(int id)
        {
            var photo = _appRepository.GetPhoto(id);
            var photo2 = _mapper.Map<PhotoForReturnDto>(photo);

            return Ok(photo2);
        }
        



    }
}