namespace todo.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;

    public class FamilyController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var families = await DocumentDBRepository<Family>.GetAllItemsAsync();
            return View(families);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Family family)
        {
            if (!ModelState.IsValid) return View(family);
            await DocumentDBRepository<Family>.CreateItemAsync(family);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Family family)
        {
            if (!ModelState.IsValid) return View(family);
            await DocumentDBRepository<Family>.UpdateItemAsync(family.Id, family);
            return RedirectToAction("Index");
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var family = await DocumentDBRepository<Family>.GetItemAsync(id);
            if (family == null)
            {
                return HttpNotFound();
            }

            return View(family);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var family = await DocumentDBRepository<Family>.GetItemAsync(id);
            if (family == null)
            {
                return HttpNotFound();
            }

            return View(family);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(string id)
        {
            await DocumentDBRepository<Family>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            var family = await DocumentDBRepository<Family>.GetItemAsync(id);
            return View(family);
        }

        [HttpPost]
        [ActionName("Search")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SearchAsync(string filter)
        {
            var families = await DocumentDBRepository<Family>.SearhcItemsAsync(x => x.LastName.Contains(filter) ||
                                                                               x.Children.FirstName.Contains(filter) ||
                                                                               x.Children.Gender.Contains(filter) ||
                                                                               x.Children.Pets.GivenName.Contains(filter) ||
                                                                               x.Parents.FirstName.Contains(filter));
            return View("Index", families);
        }

    }
}