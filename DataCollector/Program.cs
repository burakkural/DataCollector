using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models;
using NewsCollector;
using NLog;

namespace DataCollector
{
    class Program
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {

                //HaberlerCom haberlerCom = new HaberlerCom(_logger);
                //EnsonhaberCom ensonhaberCom = new EnsonhaberCom(_logger);
                NtvCom ntvCom = new NtvCom(_logger);

                List<Models.News> news = ntvCom.Collect();
                using (DataCollectorEntities db = new DataCollectorEntities())
                {
                    foreach (var item in news)
                    {
                        if (db.News.Any(x => x.Title == item.Title && x.Description == item.Description))
                            continue;

                        db.News.Add(new DataAccess.News
                        {
                            Title = item.Title,
                            Description = item.Description,
                            Url = item.Url,
                            ImageUrl = item.ImageUrl,
                            Source = (int)item.Source,
                            CreatedOn = DateTime.Now,
                            IsDeleted = false

                        });

                        db.SaveChanges();
                    }
                }

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                   String exMessage = String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    _logger.Log(LogLevel.Error, exMessage);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        String validationErrorsMsg = String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        _logger.Log(LogLevel.Error, validationErrorsMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
        }
    }
}
