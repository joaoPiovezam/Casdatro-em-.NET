using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teste.Data;
using Teste.Models;

namespace Teste.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return View(await _context.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rg,CPF,Address,AddAddress,CEP,Email,Phone")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                if (ValidateCpf(userModel.CPF) || ValidateCnpj(userModel.CPF))
                {
                    _context.Add(userModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rg,CPF,Address,AddAddress,CEP,Email,Phone")] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }
            if (ValidateCpf(userModel.CPF) || ValidateCnpj(userModel.CPF)){
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(userModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserModelExists(userModel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var userModel = await _context.Users.FindAsync(id);
            if (userModel != null)
            {
                _context.Users.Remove(userModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
          return _context.Users.Any(e => e.Id == id);
        }

        public bool ValidateCpf(string cpf)
        {
            bool valid = true;
            if (cpf.Length != 11) { 
                    valid = false;
                }
                else
                {
                    int[] mult1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                    int[] mult2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                    string tempCpf;
                    string digit;
                    int sum;
                    int remainder;

                    if (cpf.Length != 11)
                        valid = false;

                    tempCpf = cpf.Substring(0, 9);
                    sum = 0;

                    for (int i = 0; i < 9; i++)
                        sum += int.Parse(tempCpf[i].ToString()) * mult1[i];

                    remainder = sum % 11;

                    if (remainder < 2)
                        remainder = 0;
                    else
                        remainder = 11 - remainder;

                    digit = remainder.ToString();
                    tempCpf = tempCpf + digit;
                    sum = 0;

                    for (int i = 0; i < 10; i++)
                        sum += int.Parse(tempCpf[i].ToString()) * mult2[i];

                    remainder = sum % 11;

                    if (remainder < 2)
                        remainder = 0;
                    else
                        remainder = 11 - remainder;

                    digit = digit + remainder.ToString();
                    valid = cpf.EndsWith(digit);
            }
            
            return valid;
        }

        public static bool ValidateCnpj(string cnpj)
        {
            bool valid = true;

            if (cnpj.Length != 14)
            {
                valid = false;
            }
            else
            {
                int[] mult1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] mult2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int sum;
                int remaider;
                string digit;
                string tempCnpj;

                tempCnpj = cnpj.Substring(0, 12);
                sum = 0;

                for (int i = 0; i < 12; i++)
                    sum += int.Parse(tempCnpj[i].ToString()) * mult1[i];

                remaider = (sum % 11);

                if (remaider < 2)
                    remaider = 0;
                else
                    remaider = 11 - remaider;

                digit = remaider.ToString();
                tempCnpj = tempCnpj + digit;
                sum = 0;

                for (int i = 0; i < 13; i++)
                    sum += int.Parse(tempCnpj[i].ToString()) * mult2[i];

                remaider = (sum % 11);

                if (remaider < 2)
                    remaider = 0;
                else
                    remaider = 11 - remaider;

                digit = digit + remaider.ToString();
                valid = cnpj.EndsWith(digit);
            }
            return valid;
        }
    }
}
