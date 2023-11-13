using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    public class NumeroVillaController : Controller
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly INumeroVillaRepository _numerovillaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepository villaRepo, INumeroVillaRepository numerovillaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numerovillaRepo = numerovillaRepo;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener Los Numeros Villas");

                IEnumerable<NumeroVilla> numerovillaList = await _numerovillaRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numerovillaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }


        [HttpGet("id:int", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Numero de Villa con id " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;

                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(d => d.Id == id);

                //var villa = await _db.Villas.FirstOrDefaultAsync(d => d.Id == id);

                var numeroVilla = await _numerovillaRepo.Obtener(d => d.VillaNo == id);


                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsExitoso = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                //if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
                //{
                //    ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe");
                //    return BadRequest(ModelState);
                //}

                if (await _numerovillaRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe");

                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsExitoso = false;

                    return BadRequest(_response);
                }

                if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El id de la Villa no existe");

                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsExitoso = false;

                    return BadRequest(_response);
                }


                if (createDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsExitoso = false;

                    return BadRequest(_response);
                }

                //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

                //VillaStore.villaList.Add(villaDto);

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

                //Villa modelo = new()
                //{
                //    Nombre = createDto.Nombre,
                //    Detalle = createDto.Detalle,
                //    ImagenUrl = createDto.ImagenUrl,
                //    Ocupantes = createDto.Ocupantes,
                //    Tarifa = createDto.Tarifa,
                //    MetrosCuadrados = createDto.MetrosCuadrados,
                //    Amenidad = createDto.Amenidad
                //};

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _numerovillaRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsExitoso = true;

                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;

                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

                var numerovilla = await _numerovillaRepo.Obtener(v => v.VillaNo == id);

                if (numerovilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;

                    return NotFound(_response);
                }

                //VillaStore.villaList.Remove(villa);

                await _numerovillaRepo.Remover(numerovilla);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsExitoso = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return BadRequest(_response);
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {

            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;

                    return BadRequest(_response);
                }


                if (await _villaRepo.Obtener(v=> v.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea","El Id de la Villa No existe");
                    return BadRequest(ModelState);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

                //var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

                //if (villa == null)
                //{
                //    return NotFound();
                //}

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

                //Villa modelo = new()
                //{
                //    Id = updateDto.Id,
                //    Nombre = updateDto.Nombre,
                //    Detalle = updateDto.Detalle,
                //    ImagenUrl = updateDto.ImagenUrl,
                //    Ocupantes = updateDto.Ocupantes,
                //    Tarifa = updateDto.Tarifa,
                //    MetrosCuadrados = updateDto.MetrosCuadrados,
                //    Amenidad = updateDto.Amenidad
                //};


                await _numerovillaRepo.Actualizar(modelo);
                //villa.Nombre = modelo.Nombre;
                //villa.Ocupantes = modelo.Ocupantes;
                //villa.MetrosCuadrados = modelo.MetrosCuadrados;
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsExitoso = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return BadRequest(_response);
        }
        
    }
}
