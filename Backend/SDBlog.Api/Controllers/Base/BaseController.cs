
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SDBlog.BusinessLayer.Interfaces.Base;
using SDBlog.Core.Base;
using SDBlog.Core.Classes;
using SDBlog.DataModel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SDBlog.Api.Controllers.Base
{
    [Route("Api/[controller]")]
    public abstract class BaseController<TEntity, TDto> : ControllerBasico, IBaseController<TDto>
    where TEntity :  EntityBase, new()
    where TDto : DtoBase, new()
    {

        protected readonly IBaseRepository<TEntity> _db;
        protected readonly IMapper _mapper;


        protected BaseController(IBaseRepository<TEntity> service, IMapper mapper)
        {
            _db = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Listado de elementos.
        /// </summary>
        /// <returns>Retorna todos los elementos de la entidad.</returns>
        /// <response code="200">Retorna de los elementos de forma exitosa.</response>
        /// <response code="400">Error al buscar los elementos.</response>   
        /// <response code="500">Error interno.</response>   
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var list = await _db.GetAllAsync();

                var model = _mapper.Map<List<TDto>>(list);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha ocurrido un error en el método para obtener todos los datos.", false, System.Net.HttpStatusCode.InternalServerError, ex));

            }
        }


        /// <summary>
        /// Elemento por id interno.
        /// </summary>
        /// <returns>Retorna el elemento que sea igual al id interno.</returns>
        /// <response code="200">Retorna el elemento de forma exitosa.</response>
        /// <response code="400">Error al buscar el elemento.</response>
        /// <response code="500">Error interno.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var reqResult = await _db.GeTModelByIdAsync(id);

                if (reqResult == null)
                {
                    var result = new OperationResult<TDto>()
                    {
                        Result = null,
                        Success = false,
                        StatusCode = HttpStatusCode.NoContent,
                        Message = "El registro no existe."
                    };
                    return Ok(result);
                }

                var model = _mapper.Map<TDto>(reqResult);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha pcurrido un error en el método obtener datos por el Id.", false, HttpStatusCode.InternalServerError, ex));
            }

        }


        /// <summary>
        /// Agregar un elemento a la entidad.
        /// </summary>
        /// <returns>Retorna un mensaje de respuesta para confirmar el éxito de la operación si el elemento registrado cumple las validaciones.</returns>
        /// <response code="201">Registro exitoso.</response>
        /// <response code="400">Error al registrar el elemento.</response>  
        /// <response code="500">Error interno.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new OperationResult() { StatusCode = HttpStatusCode.BadRequest });

                var model = _mapper.Map<TEntity>(dto);

                var resultRepository = await _db.Add(model);
                if (!resultRepository.Success)
                    return BadRequest(resultRepository);

                var resultSave = await _db.SaveAsync();
                if (!resultSave.Success)
                    return BadRequest(resultSave);


                var result = new OperationResult<TDto>()
                {
                    Result = _mapper.Map<TDto>(model),
                    Success = true,
                    StatusCode = HttpStatusCode.Created
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha ocurrido un error en el método Insertar.", false, HttpStatusCode.InternalServerError, ex));
            }
        }


        /// <summary>
        /// Modificar un elemento en la entidad.
        /// </summary>
        /// <returns>Retorna un mensaje de respuesta para confirmar el éxito de la operación si el elemento modificado cumple las validaciones.</returns>
        /// <response code="200">Modificación exitosa.</response>
        /// <response code="400">Error al modificar el elemento.</response> 
        /// <response code="500">Error interno.</response>
        [HttpPut("{key}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public virtual async Task<IActionResult> Put(int key, [FromBody] TDto dto)
        {
            try
            {
                if (dto == null || key < 1)
                    return BadRequest(new OperationResult() { StatusCode = HttpStatusCode.BadRequest });

                var find = await _db.Find(key);
                if (find == null)
                    return NotFound(new OperationResult() { StatusCode = HttpStatusCode.NotFound });

                var model = _mapper.Map<TEntity>(dto);
                var result = _db.Update(model);
                if (result.Success)
                {
                    var resultSave = await _db.SaveAsync();
                    if (!resultSave.Success)
                        return BadRequest(resultSave);

                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha ocurrido un error en método Actualizar.", false, HttpStatusCode.InternalServerError, ex));
            }
        }

        /// <summary>
        /// Borrar un elemento en la entidad (Soft).
        /// </summary>
        /// <returns>Retorna un mensaje de respuesta para confirmar el éxito de la operación si cumple las restrinciones.</returns>
        /// <response code="200">Borrado exitoso.</response>
        /// <response code="400">Error al borrar el elemento.</response> 
        /// <response code="500">Error interno.</response>
        [HttpDelete("{key}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public virtual async Task<IActionResult> Delete(int key)
        {
            try
            {
                if (key < 1)
                    return BadRequest(new OperationResult() { StatusCode = HttpStatusCode.BadRequest });

                var model = await _db.Find(key);
                if (model == null)
                    return NotFound(new OperationResult() { StatusCode = HttpStatusCode.NotFound });

                //
                var result = _db.Remove(model);
                if (result.Success)
                {
                    var resultSave = await _db.SaveAsync();
                    if (!resultSave.Success)
                        return BadRequest(resultSave);

                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha ocurrido un error en el método Eliminar.", false, HttpStatusCode.InternalServerError, ex));
            }
        }

        /// <summary>
        /// Modificar un atributo de elemento en la entidad (Soft).
        /// </summary>
        /// <returns>Retorna un mensaje de respuesta para confirmar el éxito de la operación si cumple las validaciones.</returns>
        /// <response code="200">Modificación exitosa.</response>
        /// <response code="400">Error al modificar el elemento.</response> 
        /// <response code="500">Error interno.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TDto> jsonPatch)
        {
            try
            {
                if (jsonPatch == null && id <= 0)
                    return BadRequest();

                var model = _mapper.Map<TDto>(await _db.Find(id));

                if (model == null)
                    return BadRequest();

                jsonPatch.ApplyTo(model);

                var modelPatched = _mapper.Map<TEntity>(model);
                var result = _db.Update(modelPatched);

                if (!result.Success)
                    return BadRequest();


                var resultSave = await _db.SaveAsync();
                if (!resultSave.Success)
                    return BadRequest(resultSave);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha ocurrido un error en el método Patch.", false, HttpStatusCode.InternalServerError, ex));
            }
        }


        /// <summary>
        /// Retorna colección con paginador.
        /// </summary>
        /// <returns> Retorna listado en el cual se puede visualizar por pagina y setear el tamaño de estas paginas</returns>
        /// <response code="200">Respuesta exitosa.</response>
        /// <response code="400">Error al buscar elementos.</response> 
        /// <response code="500">Error interno.</response>
        [HttpGet("Paginator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Paginator([FromQuery] PaginatorBase paginFilter)
        {
            try
            {
                var list = await _db.GetPagedAsync(paginFilter.Page, paginFilter.Take, x => x.OrderByDescending(x => x.Id)
                , null
                );

                var model = _mapper.Map<PageCollection<TDto>>(list);
                return Ok(model);

            }
            catch (Exception ex)
            {
                return BadRequest(Respuesta("Ha ocurrido un error en el método Paginador Genérico.", false, HttpStatusCode.InternalServerError, ex));
            }
        }

    }
}
