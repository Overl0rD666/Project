using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using УчетПлатежей.Models;

namespace УчетПлатежей.Controllers
{
    public class HomeController : Controller
    {
        PaymentContext db;
        public HomeController(PaymentContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Withdraw(string name, string str)
        {
            ViewBag.str = null;
            if (str != null) ViewBag.str = str;

            IQueryable<Payment> payments = db.Payments;

            if (!String.IsNullOrEmpty(name))
            {
                payments = payments.Where(p => p.FullName.Contains(name));
            }

            PaymentListViewModel plvm = new PaymentListViewModel
            {
                Payments = payments,
                FullName = name
            };
            return View(plvm);
        }

        [HttpGet]
        public IActionResult Create(int pc, bool? add, int? pay_id, Payment payment)
        {
            if (add == true)
                ViewBag.NPos = ++pc;
            else if (add == false)
                ViewBag.NPos = pc - 1 < 1 ? 1 : --pc;
            else
            {
                ViewBag.NPos = 1;
                payment = new Payment();
            }
            return View(payment);
        }
        [HttpPost]
        public IActionResult Create(Payment payment, int pay_id, List<IFormFile> file, List<double> Claim, List<string> Text)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                double SSum = 0;

                for (int i = 0; i < Claim.Count(); i++)
                {
                    Position pos = new Position { PaymentId = payment.PaymentId };

                    if (i < file.Count() && file[i] != null)
                    {
                        byte[] file_byte = null;

                        //считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(file[i].OpenReadStream()))
                        {
                            file_byte = binaryReader.ReadBytes((int)file[i].Length);
                        }

                        //установка массива байтов
                        pos.FileByte = file_byte;
                        pos.FileType = file[i].ContentType;
                        pos.FileName = file[i].FileName;
                    }
                    pos.Claim = Claim[i];
                    SSum += Claim[i];
                    pos.Text = Text[i];
                    db.Positions.Add(pos);
                }

                payment.Sum = SSum;

                db.SaveChanges();
                return RedirectToAction("Withdraw", new { str = "Заявка, " + payment.FullName + ", была успешно создана!" });
            }
            else
            {
                ViewBag.NPos = 1;
                return View(payment);
            }

        }

        public async Task<IActionResult> Edit(int pc, bool? add, int? id, int pos_id)
        {
            if (id != null)
            {
                Payment payment = await db.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
                if (payment != null)
                {
                    payment.Positions = (from pos in db.Positions where pos.PaymentId == id select pos).ToList();

                    if (add == true) payment.Positions.Add(new Position());
                    else if (add == false)
                    {
                        Position pos = await db.Positions.FirstOrDefaultAsync(p => p.PositionId == pos_id);
                        switch (payment.Positions.Count())
                        {
                            case 0:
                                payment.Positions.Add(new Position());
                                break;
                            case 1:
                                payment.Positions.Remove(pos);
                                payment.Positions.Add(new Position());
                                break;
                            default:
                                payment.Positions.Remove(pos);
                                break;
                        }
                    }

                    return View(payment);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Payment payment, List<double> Claims, List<string> Texts, List<int> PositionId)
        {
            if (ModelState.IsValid)
            {
                payment.Positions = db.Positions.Where(p => p.PaymentId == payment.PaymentId && PositionId.Contains(p.PositionId)).ToList();

                List<Position> del_pos = db.Positions.Where(p => p.PaymentId == payment.PaymentId && !PositionId.Contains(p.PositionId)).ToList();
                foreach(var pos in del_pos)
                {
                    db.Entry(pos).State = EntityState.Deleted;
                }

                db.Payments.Update(payment);
                double SSum = 0;

                for (int i = 0; i < Claims.Count(); i++)
                {
                    if (i < payment.Positions.Count)
                    {
                        payment.Positions[i].Claim = Claims[i];
                        payment.Positions[i].Text = Texts[i];
                    }
                    else
                    {
                        Position pos = new Position {
                            PaymentId = payment.PaymentId,
                            Claim = Claims[i],
                            Text = Texts[i]
                        };
                        payment.Positions.Add(pos);
                    }
                    SSum += Claims[i];
                }
                payment.Sum = SSum;

                await db.SaveChangesAsync();

                return RedirectToAction("Withdraw", new { str = "Изменения были сохранены!" });
            }
            else
            {
                return View(payment);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Payment payment = await db.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
                if (payment != null)
                    return View(payment);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Payment payment = new Payment { PaymentId = id.Value };
                db.Entry(payment).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Withdraw", new { str = "Заявка была удалена!" });
            }
            return NotFound();
        }

        [HttpGet]
        // Отправка массива байтов
        public async Task<IActionResult> GetBytes(int? id)
        {
            if (id != null)
            {
                Position position = await db.Positions.FirstOrDefaultAsync(p => p.PositionId == id);
                if (position != null)
                {
                    return File(position.FileByte, position.FileType, position.FileName);
                }
            }
            return NotFound();
        }

    }
}
