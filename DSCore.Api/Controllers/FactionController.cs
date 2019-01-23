﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using DSCore.Ini;

namespace DSCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactionController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            if (!Utils.CheckDatabase())
                return Utils.ReturnJson(null, Errors.DatabaseNotFound);

            using (var db = new LiteDatabase(Utils.GetDatabase()))
            {
                var collection = db.GetCollection<Faction>("Factions");
                if (collection.Count() == 0)
                    return Utils.ReturnJson(null, Errors.InvalidDatabaseStructure);

                return Utils.ReturnJson(collection.FindAll(), Errors.Null);
            }
        }

        // GET api/faction/fc_n_grp
        [HttpGet("{nickname}")]
        public ActionResult<string> Get(string nickname)
        {
            if (!Utils.CheckDatabase())
                return Utils.ReturnJson(null, Errors.DatabaseNotFound);

            using (var db = new LiteDatabase(Utils.GetDatabase()))
            {
                var collection = db.GetCollection<Faction>("Factions");
                if (collection.Count() == 0)
                    return Utils.ReturnJson(null, Errors.InvalidDatabaseStructure);

                var find = collection.FindById(nickname);
                if (find == null)
                    return Utils.ReturnJson(null, Errors.ValueNotFound);
                else
                {
                    return Utils.ReturnJson(find, Errors.Null);
                }
            }
        }
    }
}