using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expenses.Models;
using Expenses.Models.ExpenseViewModels;

namespace Expenses.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ExpensesContext _context;

        public ExpensesController(ExpensesContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            ViewData["InteractorId"] = new SelectList(_context.Set<Interactor>(), "InteractorId", "Name");
            var expensesContext = _context.Expense.Include(e => e.Interactor);
            return View(await expensesContext.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .Include(e => e.Interactor)
                .Include(e => e.ExpenseImages)
                .SingleOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["InteractorId"] = new SelectList(_context.Set<Interactor>(), "InteractorId", "Name");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,Name,Description,Created,DateFor,Value,Paid,PaidDate,PaidValue,IAmPayer,InteractorId")] Expense expense)
        {
            expense.Created = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InteractorId"] = new SelectList(_context.Set<Interactor>(), "InteractorId", "Name", expense.InteractorId);
            return View(expense);
        }

        [HttpPost]
        public async Task<JsonResult> CreateInteractor(string interactorName)
        {
            if (!string.IsNullOrEmpty(interactorName))
            {
                _context.Add(new Interactor { Name = interactorName });
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["InteractorId"] = new SelectList(_context.Set<Interactor>(), "InteractorId", "Name", expense.InteractorId);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId,Name,Description,Created,DateFor,Value,Paid,PaidDate,PaidValue,IAmPayer,InteractorId")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
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
            ViewData["InteractorId"] = new SelectList(_context.Set<Interactor>(), "InteractorId", "Name", expense.InteractorId);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .Include(e => e.Interactor)
                .SingleOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ExpenseId == id);
            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expense.Any(e => e.ExpenseId == id);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayForExpense([Bind("ExpenseId,PaidDate,PaidValue")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingExpense = _context.Expense.Find(expense.ExpenseId);
                    existingExpense.PaidDate = expense.PaidDate ?? existingExpense.DateFor;
                    existingExpense.PaidValue = expense.PaidValue ?? existingExpense.Value;
                    existingExpense.Paid = true;

                    _context.Update(existingExpense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["InteractorId"] = new SelectList(_context.Set<Interactor>(), "InteractorId", "Name", expense.InteractorId);
            return RedirectToAction(nameof(Index));
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImages(int expenseId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var ms = new MemoryStream();
                        file.OpenReadStream().CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var expenseImage = new ExpenseImage
                        {
                            ExpenseId = expenseId,
                            Image = fileBytes
                        };
                        _context.Add(expenseImage);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Details), new {id = expenseId});
        }
    }
}
