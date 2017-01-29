namespace todo.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;

    public class ItemController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(d => !d.Completed);
            return View(items);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Description,Completed,Title")] Item item)
        {
            if (!ModelState.IsValid) return View(item);
            await DocumentDBRepository<Item>.CreateItemAsync(item);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Description,Completed,Title")] Item item)
        {
            if (!ModelState.IsValid) return View(item);
            await DocumentDBRepository<Item>.UpdateItemAsync(item.Id, item);
            return RedirectToAction("Index");
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            await DocumentDBRepository<Item>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            var item = await DocumentDBRepository<Item>.GetItemAsync(id);
            return View(item);
        }

        [HttpPost]
        [ActionName("Search")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SearchAsync(string filter)
        {
            var items = await DocumentDBRepository<Item>.SearhcItemsAsync(x => x.Description.Contains(filter));

            return View("Index", items);

        }

        [ActionName("CreateUDF")]
        public ActionResult CreateUDFsAsync()
        {
            DocumentDBRepository<Item>.CreateToUpperUdfAsync();
            return JavaScript("<script>alert(\"FunctionCreated\")</script>");
        }

       
    }
}