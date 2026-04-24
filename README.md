# Learning Management System (LMS)

A Learning Management System similar to Canvas, built across 4 phases for CS 5530 (Database Systems) at the University of Utah. The frontend UI was provided; the project focused on database design, SQL schema creation, backend controller implementation, and cloud deployment.

## Tech Stack

- **Languages:** C#, HTML, CSS, JavaScript
- **Framework:** ASP.NET Core (Blazor / MVC)
- **Database:** MySQL
- **ORM:** Entity Framework Core (LINQ)
- **Authentication:** ASP.NET Identity
- **IDE:** Visual Studio
- **Deployment:** Linux VM (CADE), HTTPS via self-signed certificate
- **Team:** 2 Developers

## Features

- Role-based access for **Students**, **Professors**, and **Administrators**
- Administrators can create departments and courses
- Professors can create classes, assignment categories, assignments, and grade submissions
- Students can view enrollments, submit assignments, and track grades
- **Automatic grade calculation:** letter grades computed from weighted assignment categories whenever a submission is graded or a new assignment is created
- User authentication and login via ASP.NET Identity
- Full deployment to a live Linux web server accessible via browser

## Project Phases

### Phase 1: ER Modeling
- Designed a full Entity-Relationship diagram for the LMS in Chen notation
- Modeled users (Student, Professor, Administrator), Departments, Courses, Classes, Assignments, Assignment Categories, and Submissions with appropriate cardinality and participation constraints

### Phase 2: SQL Schema
- Translated the ER diagram into normalized MySQL tables
- Defined primary keys, foreign keys, and uniqueness constraints (e.g. course number unique per department, class offering unique per semester)
- Data types chosen to match spec (uIDs as `u` + 7 digits, semester as season + unsigned int year, etc.)

### Phase 3: Backend Controllers
- Scaffolded MySQL tables into C# models using Entity Framework Core
- Implemented controller methods across `CommonController`, `ProfessorController`, `StudentController`, and `AdminController`
- Wrote LINQ queries for all LMS operations including enrollment, assignment management, and grade calculation
- Implemented automatic letter grade recalculation with weighted category scaling whenever assignments are created or submissions are graded

### Phase 4: Deployment
- Deployed the web server to a CADE-hosted Linux VM
- Configured user secrets for secure database credential management (no hard-coded passwords)
- Used `tmux` to keep the server running persistently after SSH logout
- Verified live access via browser over HTTPS

## Architecture

```
View (Razor Pages) → Controllers (LINQ/EF Core) → MySQL (TeamXLMS)
                                                 → ASP.NET Identity (TeamXLMSUsers)
```

## Authors

Developed with a partner for CS 5530 (Database Systems), University of Utah (Spring 2026).
