using GenericApi.Services;
using GenericApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// this base Generic api Controller takes three paremeters DbContect model Class and model class Id type eg .. int 
namespace GenericApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   public abstract  class DefaultGenericController<DbEntity, TEntity, TKey> : ControllerBase where DbEntity : DbContext where TEntity : class where TKey : notnull
    {
        
        private readonly IRepositoryAsync<DbEntity,TEntity> db;
        //private readonly IUnitOfWork<DbEntity> db;
       public DefaultGenericController(IRepositoryAsync<DbEntity,TEntity> repo)=>  db = repo;
        
        
        // UnitOfWork
         //public DefaultGenericController(IUnitOfWork<DbEntity> repo)=> db = repo;
        
        // GET: api/Generic
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetItems()
        {
            var items = await db.GetListAsync();
            return Ok(items);
        }

        // GET: api/Generic/5
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> GetItem(TKey id)
        {
            if(id == null)  return NotFound();

            var item = await db.FindByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Generic/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutItem(TKey id, TEntity item)
        {
            if (!ModelState.IsValid || id == null)
            {
                return BadRequest(ModelState.ErrorMessages());
            }
            dynamic ItemBox = item;

            if (id != ItemBox.Id)
            {
                return BadRequest();
            }

            db.Update(item);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var obj = await db.FindByIdAsync(id);
                if (obj == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> PostItem(TEntity item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorMessages());
            }
            
            await db.AddAsync(item);
            await db.SaveChangesAsync();

            dynamic ItemBox = item;
            return CreatedAtAction("GetItem", new { id = ItemBox.Id }, item);
        }

        // DELETE: api/item/5
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteItem(TKey id)
        {
            if(id == null)  return BadRequest();    

            var item = await db.FindByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Delete(item);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
