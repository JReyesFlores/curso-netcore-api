using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JMusik.Data;
using JMusik.Models;
using JMusik.Data.Contratos;
using AutoMapper;
using JMusik.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace JMusik.WebAPI.Controllers
{
    [Authorize(Roles = "Administrador,Vendedor")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        //private readonly TiendaDbContext _context;
        private readonly IProductosRepositorio _productosRepositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductosRepositorio productosRepositorio,
                                   IMapper mapper,
                                   ILogger<ProductosController> logger)
        {
            this._productosRepositorio = productosRepositorio;
            this._mapper = mapper;
            this._logger = logger;
        }

        //// GET: api/Productos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            try
            {
                var productos = await this._productosRepositorio.ObtenerProductosAsync();
                return this._mapper.Map<List<ProductoDto>>(productos);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error al obtener los productos: {ex.Message}");
                return BadRequest();
            }
        }

        //// GET: api/Productos/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var producto = await this._productosRepositorio.ObtenerProductoAsync(id);
            if (producto == null) return NotFound();
            return this._mapper.Map<ProductoDto>(producto);
        }

        //// POST: api/Productos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Post(ProductoDto productoDto)
        {
            try
            {
                var producto = this._mapper.Map<Producto>(productoDto);

                var nuevoProducto = await this._productosRepositorio.Agregar(producto);
                if (nuevoProducto == null) return BadRequest();

                var nuevoProductoDto = this._mapper.Map<ProductoDto>(nuevoProducto);
                return CreatedAtAction(nameof(Post), new { id = nuevoProductoDto.Id }, nuevoProductoDto);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error al agregar los productos: {ex.Message}");
                return BadRequest();
            }
        }

        //// PUT: api/Productos/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Put(int id, [FromBody] ProductoDto productoDto)
        {
            if (productoDto == null) return NotFound();

            var producto = this._mapper.Map<Producto>(productoDto);
            var resultado = await this._productosRepositorio.Actualizar(producto);
            if (!resultado) return BadRequest();

            return productoDto;
        }


        //// DELETE: api/Productos/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await this._productosRepositorio.Eliminar(id);
                if (!resultado) return BadRequest();
                return NoContent();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error al eliminar el producto: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
