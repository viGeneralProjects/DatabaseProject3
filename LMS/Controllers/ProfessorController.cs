using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo( "LMSControllerTests" )]
namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var students = db.Enrollments.Where(e => e.Class.Course.DIdNavigation.Subject == subject && e.Class.Course.Number == num && e.Class.Season == season && e.Class.Year == year).Select(e => new
            {
                fname = e.UIdNavigation.FirstName,
                lname = e.UIdNavigation.LastName,
                uid = e.UId,
                dob = e.UIdNavigation.Dob,
                grade = e.Grade
            });
            return Json(students);
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            if (category == null)
            {
                var assignments = db.Assignments
                    .Where(a => a.Category.Class.Course.DIdNavigation.Subject == subject
                            && a.Category.Class.Course.Number == num
                            && a.Category.Class.Season == season
                            && a.Category.Class.Year == year)
                    .Select(a => new
                    {
                        aname = a.Name,
                        cname = a.Category.Name,
                        due = a.DueDate,
                        submissions = a.Submissions.Count()
                    }).ToList();
                return Json(assignments);
            }
            else
            {
                var assignments = db.Assignments
                    .Where(a => a.Category.Class.Course.DIdNavigation.Subject == subject
                            && a.Category.Class.Course.Number == num
                            && a.Category.Class.Season == season
                            && a.Category.Class.Year == year
                            && a.Category.Name == category)
                    .Select(a => new
                    {
                        aname = a.Name,
                        cname = a.Category.Name,
                        due = a.DueDate,
                        submissions = a.Submissions.Count()
                    }).ToList();
                return Json(assignments);

            }
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var categories = db.AssignmentCategories.Where(ac => ac.Class.Course.DIdNavigation.Subject == subject && ac.Class.Course.Number == num && ac.Class.Season == season && ac.Class.Year == year).Select(ac => new { name = ac.Name, weight = ac.Weight });
            return Json(categories);
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            uint classId = db.Classes.Where(c => c.Course.DIdNavigation.Subject == subject && c.Course.Number == num && c.Season == season && c.Year == year).Select(c => c.ClassId).FirstOrDefault();
            if (db.AssignmentCategories.Any(ac => ac.ClassId == classId && ac.Name == category))
            {
                return Json(new { success = false });
            }
            db.AssignmentCategories.Add(new AssignmentCategory
            {
                ClassId = classId,
                Name = category,
                Weight = (uint)catweight
            });
            db.SaveChanges();
            return Json(new { success = true });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            uint categoryId = db.AssignmentCategories.Where(ac => ac.Class.Course.DIdNavigation.Subject == subject && ac.Class.Course.Number == num && ac.Class.Season == season && ac.Class.Year == year && ac.Name == category).Select(ac => ac.CategoryId).FirstOrDefault();
            if (db.Assignments.Any(a => a.CategoryId == categoryId && a.Name == asgname))
            {
                return Json(new { success = false });
            }
            db.Assignments.Add(new Assignment
            {
                CategoryId = categoryId,
                Name = asgname,
                MaxPoint = (uint)asgpoints,
                DueDate = asgdue,
                Contents = asgcontents

            });
            db.SaveChanges();
            // Auto-grade
            uint classId = db.AssignmentCategories.Where(ac => ac.CategoryId == categoryId).Select(ac => ac.ClassId).FirstOrDefault();
            var enrolledStudents = db.Enrollments.Where(e => e.ClassId == classId).Select(e => e.UId).ToList();
            foreach (var studentUid in enrolledStudents)
            {
                UpdateGrade(studentUid, classId);
            }
            return Json(new { success = true });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var submissions = db.Submissions
                .Where(s => s.Assign.Category.Class.Course.DIdNavigation.Subject == subject
                && s.Assign.Category.Class.Course.Number == num
                && s.Assign.Category.Class.Season == season
                && s.Assign.Category.Class.Year == year
                && s.Assign.Category.Name == category
                && s.Assign.Name == asgname)
                .Select(s => new
                {
                    fname = s.UIdNavigation.FirstName,
                    lname = s.UIdNavigation.LastName,
                    uid = s.UId,
                    time = s.Time,
                    score = s.Score
                }).ToList();
            return Json(submissions);
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            var submission = db.Submissions.FirstOrDefault(s => s.Assign.Category.Class.Course.DIdNavigation.Subject == subject && s.Assign.Category.Class.Course.Number == num && s.Assign.Category.Class.Season == season && s.Assign.Category.Class.Year == year && s.Assign.Category.Name == category && s.Assign.Name == asgname && s.UId == uid);
            if (submission == null)
            {
                return Json(new { success = false });
            }
            submission.Score = (uint)score;
            db.SaveChanges();
            //Auto-grade
            uint classId = db.Classes.Where(c => c.Course.DIdNavigation.Subject == subject && c.Course.Number == num && c.Season == season && c.Year == year).Select(c => c.ClassId).FirstOrDefault();
            UpdateGrade(uid, classId);
            return Json(new { success = true });
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var classes = db.Classes.Where(c => c.TaughtBy == uid).Select(c => new
            {
                subject = c.Course.DIdNavigation.Subject,
                number = c.Course.Number,
                name = c.Course.Name,
                season = c.Season,
                year = c.Year
            });
            return Json(classes);
        }

        //helper methods for auto-grade
        private string LetterGrade(double score)
        {
            if (score >= 93) return "A";
            if (score >= 90) return "A-";
            if (score >= 87) return "B+";
            if (score >= 83) return "B";
            if (score >= 80) return "B-";
            if (score >= 77) return "C+";
            if (score >= 73) return "C";
            if (score >= 70) return "C-";
            if (score >= 67) return "D+";
            if (score >= 63) return "D";
            if (score >= 60) return "D-";
            else return "E";
        }
        private void UpdateGrade(string uid, uint classId)
        {
            var categories = db.AssignmentCategories.Where(ac => ac.ClassId == classId && ac.Assignments.Any()).ToList();
            if (!categories.Any())
            {
                var enrollment = db.Enrollments.FirstOrDefault(e => e.UId == uid && e.ClassId == classId);
                if (enrollment != null)
                {
                    enrollment.Grade = "--";
                    db.SaveChanges();
                }
                return;
            }
            double totalScaled = 0;
            double totalWeight = 0;
            foreach (var category in categories)
            {
                var assignments = db.Assignments.Where(a => a.CategoryId == category.CategoryId).ToList();
                double totalEarned = 0;
                double totalMax = 0;
                foreach (var assignment in assignments)
                {
                    var submission = db.Submissions.FirstOrDefault(s => s.AssignId == assignment.AssignId && s.UId == uid);
                    totalEarned += submission != null ? submission.Score : 0;
                    totalMax += assignment.MaxPoint;
                }
                double categoryPercentage = totalEarned / totalMax;
                totalScaled += categoryPercentage * category.Weight;
                totalWeight += category.Weight;
            }
            double scalingFactor = 100.0 / totalWeight;
            double finalScore = totalScaled * scalingFactor;
            string grade = LetterGrade(finalScore);
            var enroll = db.Enrollments.FirstOrDefault(e => e.UId == uid && e.ClassId == classId);
            if (enroll != null)
            {
                enroll.Grade = grade;
                db.SaveChanges();
            }
        }
            /*******End code to modify********/
    }
}

