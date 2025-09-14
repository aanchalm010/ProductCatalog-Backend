//using Microsoft.AspNetCore.Mvc;
//using ProductCatalog.Api.Dtos;
//using ProductCatalog.Api.Services;

//namespace ProductCatalog.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductsController : ControllerBase
//    {
//        private readonly IProductService _svc;
//        public ProductsController(IProductService svc) => _svc = svc;

//        //list all products
//        // GET /api/products
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAll()
//        {
//            var items = await _svc.GetAllAsync();
//            return Ok(items); 
//        }

//        //get a specific product by id
//        // GET /api/products/{id}
//        [HttpGet("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Get(int id)
//        {
//            var item = await _svc.GetByIdAsync(id);
//            return item == null ? NotFound() : Ok(item);
//        }

//        //create a new product
//        // POST /api/products
//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var created = await _svc.CreateAsync(dto);
//            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
//        }


//        //update an existing product
//        // PUT /api/products/{id}
//        [HttpPut("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var updated = await _svc.UpdateAsync(id, dto);
//            if (updated == null) return NotFound();

//            return Ok(updated);
//        }

//        //delete a product
//        // DELETE /api/products/{id}
//        [HttpDelete("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var deleted = await _svc.DeleteAsync(id);
//            return deleted ? NoContent() : NotFound();
//        }

//        //upload an image for a product
//        // POST /api/products/{id}/image
//        [HttpPost("{id:int}/image")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> UploadImage(int id, IFormFile file)
//        {
//            if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

//            var updated = await _svc.UploadImageAsync(id, file);
//            return updated == null ? NotFound() : Ok(updated); 
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Dtos;
using ProductCatalog.Api.Services;

namespace ProductCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _svc;
        public ProductsController(IProductService svc) => _svc = svc;

        // GET /api/products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _svc.GetAllAsync();
            return Ok(items);
        }

        // GET /api/products/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST /api/products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(dto);
            if (created == null) return BadRequest("Failed to create product.");

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/products/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _svc.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _svc.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // POST /api/products/{id}/image
        //[HttpPost("{id:int}/image")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

        //    var updated = await _svc.UploadImageAsync(id, file);
        //    return updated == null ? NotFound() : Ok(updated);
        //}
        [HttpPost("{id}/image")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]   
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]   
        [ProducesResponseType(StatusCodes.Status400BadRequest)]               
        [ProducesResponseType(StatusCodes.Status404NotFound)]                 
        public async Task<ActionResult<ProductDto>> UploadImage(int id, [FromForm] FileUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Invalid file.");   

            var result = await _svc.UploadImageAsync(id, dto.File);
            if (result == null)
                return NotFound();                   

            return Ok(result);                       
        }




        //------------------------------------------------
        //DELETE /api/products/{id}/image
        [HttpDelete("{id:int}/image")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveImage(int id)
    {
        var updated = await _svc.RemoveImageAsync(id);
        return updated == null ? NotFound() : Ok(updated);
    }
}
}
