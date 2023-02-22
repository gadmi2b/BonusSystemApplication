using BonusSystemApplication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.EF
{
    public static class SeedData
    {

        private static Department[] Departments =
        {
            new Department { Name = "Administration" },
            new Department { Name = "BD" },
            new Department { Name = "Engineering" },
            new Department { Name = "Finance" },
            new Department { Name = "HR" },
            new Department { Name = "IT" },
            new Department { Name = "Quality" }
        };

        private static Position[] Positions =
        {
            new Position { NameEng = "All People", NameRus = "Все сотрудники", Abbreviation = "N/A" },
            new Position { NameEng = "General Director", NameRus = "Генеральный Директор", Abbreviation = "AGD" },
            new Position { NameEng = "Facility Manager", NameRus = "Начальник АХО", Abbreviation = "AFM" },
            new Position { NameEng = "Corporate Affairs Specialist", NameRus = "Специалист по корпоративному управлению", Abbreviation = "ACA" },
            new Position { NameEng = "Travel Coordinator", NameRus = "Специалист по организации деловых поездок", Abbreviation = "ATC" },
            new Position { NameEng = "Head of Business Development", NameRus = "Директор по развитию бизнеса", Abbreviation = "HoBD" },
            new Position { NameEng = "Head of Engineering", NameRus = "Технический директор", Abbreviation = "HoE" },
            new Position { NameEng = "Design Manager", NameRus = "Руководитель конструкторского направления", Abbreviation = "ENDM" },
            new Position { NameEng = "Programme Manager", NameRus = "Руководитель направления по управлению проектами", Abbreviation = "ENPM" },
            new Position { NameEng = "Engineering Supplier Manager", NameRus = "Менеджер по работе с поставщиками", Abbreviation = "ENESM" },
            new Position { NameEng = "Chief Engineer", NameRus = "Главный конструктор", Abbreviation = "ENCD" },
            new Position { NameEng = "Stress Manager", NameRus = "Руководитель направления прочности", Abbreviation = "ENSM" },
            new Position { NameEng = "Project Leader", NameRus = "Руководитель проекта", Abbreviation = "DPL" },
            new Position { NameEng = "Design Domain Expert", NameRus = "Главный специалист конструкторского направления", Abbreviation = "DDE" },
            new Position { NameEng = "Lead Design Engineer", NameRus = "Ведущий инженер-конструктор", Abbreviation = "DLE" },
            new Position { NameEng = "Senior Design Engineer", NameRus = "Старший инженер-конструктор", Abbreviation = "DSE" },
            new Position { NameEng = "Design Engineer - Category 1", NameRus = "Инженер-конструктор 1 категории", Abbreviation = "DE1" },
            new Position { NameEng = "Design Engineer - Category 2", NameRus = "Инженер-конструктор 2 категории", Abbreviation = "DE2" },
            new Position { NameEng = "Design Engineer - Category 3", NameRus = "Инженер-конструктор 3 категории", Abbreviation = "DE3" },
            new Position { NameEng = "Design Engineer", NameRus = "Инженер-конструктор", Abbreviation = "DE" },
            new Position { NameEng = "Design Technician", NameRus = "Техник-конструктор", Abbreviation = "DT" },
            new Position { NameEng = "Project Leader (Stress)", NameRus = "Руководитель проекта (прочность)", Abbreviation = "SPL" },
            new Position { NameEng = "Stress Domain Expert", NameRus = "Главный специалист Направления прочности", Abbreviation = "SDE" },
            new Position { NameEng = "Lead Stress Engineer", NameRus = "Ведущий инженер по прочности", Abbreviation = "SLE" },
            new Position { NameEng = "Senior Stress Engineer", NameRus = "Старший инженер по прочности", Abbreviation = "SSE" },
            new Position { NameEng = "Stress Engineer - Category 1", NameRus = "Инженер 1 категории по прочности", Abbreviation = "SE1" },
            new Position { NameEng = "Stress Engineer - Category 2", NameRus = "Инженер 2 категории по прочности", Abbreviation = "SE2" },
            new Position { NameEng = "Stress Engineer - Category 3", NameRus = "Инженер 3 категории по прочности", Abbreviation = "SE3" },
            new Position { NameEng = "Stress Engineer", NameRus = "Инженер по прочности", Abbreviation = "SE" },
            new Position { NameEng = "Stress Technician", NameRus = "Техник по прочности", Abbreviation = "ST" },
            new Position { NameEng = "Analytic-Programmer", NameRus = "Аналитик-программист", Abbreviation = "PMAP" },
            new Position { NameEng = "PMO", NameRus = "Специалист по управлению проектами", Abbreviation = "PMS" },
            new Position { NameEng = "Lead PMO", NameRus = "Ведущий специалист по управлению проектами", Abbreviation = "PMLS" },
            new Position { NameEng = "Engineering Supplier Specialist", NameRus = "Специалист по работе с поставщиками", Abbreviation = "ESS" },
            new Position { NameEng = "Accountant", NameRus = "Бухгалтер", Abbreviation = "FA" },
            new Position { NameEng = "Analytic Accountant", NameRus = "Бухгалтер-аналитик", Abbreviation = "FAA" },
            new Position { NameEng = "Head of Finance", NameRus = "Финансовый директор", Abbreviation = "HoF" },
            new Position { NameEng = "Deputy Chief Accountant", NameRus = "Заместитель главного бухгалтера", Abbreviation = "FDC" },
            new Position { NameEng = "Lead Economist", NameRus = "Ведущий экономист", Abbreviation = "FLE" },
            new Position { NameEng = "Senior accountant", NameRus = "Старший бухгалтер", Abbreviation = "FSA" },
            new Position { NameEng = "Head of HR", NameRus = "Директор по персоналу", Abbreviation = "HoHR" },
            new Position { NameEng = "HR Administrator", NameRus = "Специалист по кадровому делопроизводству", Abbreviation = "HRA" },
            new Position { NameEng = "HR Specialist, Recruitment, Assessment and Motivation", NameRus = "Специалист по набору, оценке и мотивации персонала", Abbreviation = "HRR" },
            new Position { NameEng = "HR Specialist, Trainings and Internal Communications", NameRus = "Специалист по обучению и внутренним коммуникациям", Abbreviation = "HRT" },
            new Position { NameEng = "Head of IT&S", NameRus = "Директор по информационным технологиям и безопасности", Abbreviation = "HoITS" },
            new Position { NameEng = "IT Manager", NameRus = "Менеджер по информационным технологиям", Abbreviation = "ITM" },
            new Position { NameEng = "CAD/CAE/PDM Support Manager", NameRus = "Менеджер по поддержке CAD/CAE/PDM", Abbreviation = "ITCADM" },
            new Position { NameEng = "CAD/CAE/PDM Support Specialist", NameRus = "Специалист по поддержке CAD/CAE/PDM", Abbreviation = "ITCADS" },
            new Position { NameEng = "Senior UNIX System Administrator", NameRus = "Старший системный администратор UNIX инфраструктуры", Abbreviation = "ITSUA" },
            new Position { NameEng = "Windows and UNIX System Administrator", NameRus = "Системный администратор Windows и UNIX инфраструктуры", Abbreviation = "ITUWSA" },
            new Position { NameEng = "WINDOWS Infrastructure Technical Support Specialist", NameRus = "Специалист по технической поддержке WINDOWS инфраструктуры", Abbreviation = "ITWS" },
            new Position { NameEng = "DDC Head", NameRus = "Руководитель Группы управления качеством конструкторской документации", Abbreviation = "DDCHO" },
            new Position { NameEng = "Lead DDC Specialist", NameRus = "Ведущий специалист Группы управления качеством конструкторской документации", Abbreviation = "DDCLS" },
            new Position { NameEng = "DDC Specialist", NameRus = "Специалист Группы управления качеством конструкторской документации", Abbreviation = "DDCS" },
            new Position { NameEng = "Head of Quality", NameRus = "Директор по качеству", Abbreviation = "HoQ" },
            new Position { NameEng = "Quality Specialist, Methods&Processes", NameRus = "Специалист по методам и процессам в области качества", Abbreviation = "QMP" }
        };

        private static Team[] Teams =
        {
            new Team { Name = "w/o team" },
            new Team { Name = "CI" },
            new Team { Name = "DDC" },
            new Team { Name = "Design" },
            new Team { Name = "Management" },
            new Team { Name = "Stress" },
            new Team { Name = "Subcontracting" }
        };

        private static User[] Users =
        {
            //designers:
            new User { FirstNameEng = "Ivan", LastNameEng = "Ivanov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Iviva00", Email = "iviva00@airbus.com", Pid = "100dsg",
                       Department = Departments[2], Team = Teams[3], Position = Positions[16]},
            new User { FirstNameEng = "Dmitry", LastNameEng = "Dmitriev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Dmdmi01", Email = "dmdmi01@airbus.com", Pid = "101str",
                       Department = Departments[2], Team = Teams[5], Position = Positions[25]},
            new User { FirstNameEng = "Sergey", LastNameEng = "Sergeev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Seser02", Email = "seser02@airbus.com", Pid = "102dsg",
                       Department = Departments[2], Team = Teams[3], Position = Positions[18]},
            new User { FirstNameEng = "Marat", LastNameEng = "Maratov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Mamar03", Email = "dmdmi03@airbus.com", Pid = "103str",
                       Department = Departments[2], Team = Teams[5], Position = Positions[25]},

            //managers:
            new User { FirstNameEng = "Karl", LastNameEng = "Karlov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Kakar02", Email = "kakar04@airbus.com", Pid = "104mng",
                       Department = Departments[2], Team = Teams[3], Position = Positions[12]},
            new User { FirstNameEng = "Alexander", LastNameEng = "Alexandrov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Alale03", Email = "alale05@airbus.com", Pid = "105mng",
                       Department = Departments[2], Team = Teams[5], Position = Positions[12]},

            //approvers:
            new User { FirstNameEng = "Petr", LastNameEng = "Petrov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Pepet04", Email = "pepet06@airbus.com", Pid = "106apr",
                       Department = Departments[2], Team = Teams[3], Position = Positions[7]},
            new User { FirstNameEng = "Mark", LastNameEng = "Markov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Mamar05", Email = "mamar07@airbus.com", Pid = "107apr",
                       Department = Departments[2], Team = Teams[5], Position = Positions[11]},

            //administration:
            new User { FirstNameEng = "Nikolay", LastNameEng = "Nikolaev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Ninik06", Email = "ninik08@airbus.com", Pid = "108hr",
                       Department = Departments[4], Team = Teams[0], Position = Positions[42]},
            new User { FirstNameEng = "Boris", LastNameEng = "Borisov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Bobor07", Email = "bobor09@airbus.com", Pid = "109",
                       Department = Departments[0], Team = Teams[4], Position = Positions[1]},
        };

        private static Workproject[] Workprojects =
                {
            new Workproject { Name = "All", Description = "All WP (including N/A)" },
            new Workproject { Name = "13", Description = "Serial Activity SA 13-14" },
            new Workproject { Name = "82", Description = "LR S15/21 Serial Activity" },
            new Workproject { Name = "93", Description = "Test Facility" },
            new Workproject { Name = "98", Description = "A350 Crown&Cabin Bbracket" },
            new Workproject { Name = "118-0", Description = "A350 NCF Serial Activity - Leaders" },
            new Workproject { Name = "118-1", Description = "A350 NCF Serial Activity - Design" },
            new Workproject { Name = "118-2", Description = "A350 NCF Serial Activity - Stress ZM" },
            new Workproject { Name = "118-3", Description = "A350 NCF Serial Activity - Stress ZB" },
            new Workproject { Name = "119", Description = "Serial Activity AF MSI SA LR A380" },
            new Workproject { Name = "131", Description = "A350 PET Activity" },
            new Workproject { Name = "135-0", Description = "SA Stress - Task Leaders" },
            new Workproject { Name = "135-1", Description = "SA Stress - AF Static" },
            new Workproject { Name = "135-2", Description = "SA Stress - AD Static" },
            new Workproject { Name = "135-3", Description = "SA Stress" },
            new Workproject { Name = "138", Description = "SA/LR/A380 Mechanical Systems Installation" },
            new Workproject { Name = "141", Description = "A330-900F Development" },
            new Workproject { Name = "144", Description = "20-Meter Car" },
            new Workproject { Name = "145", Description = "SA/LR A-D Structure" },
            new Workproject { Name = "146", Description = "A350 Integration" },
            new Workproject { Name = "148", Description = "SA MSI Automatization" },
            new Workproject { Name = "149", Description = "DDMS SA 15-18" },
            new Workproject { Name = "150", Description = "CTP Interiors" },
            new Workproject { Name = "151", Description = "SA Belly Fairing and Floor Panels" },
            new Workproject { Name = "152", Description = "A330 F&DT Engineering Support" },
            new Workproject { Name = "153", Description = "RHC - Design" },
            new Workproject { Name = "154", Description = "A350F Barrier Wall Fittings" },
            new Workproject { Name = "155", Description = "Railway AC System" },
            new Workproject { Name = "156", Description = "RHC - Stress Audit" },
            new Workproject { Name = "157", Description = "Railway Elevator" },
            new Workproject { Name = "158", Description = "Engineering Automation" },
            new Workproject { Name = "159", Description = "TBD" },
            new Workproject { Name = "160", Description = "TBD" },
            new Workproject { Name = "AM1", Description = "Automation development" },
            new Workproject { Name = "BD1", Description = "Business Development Support & Tenders" },
            new Workproject { Name = "CE1", Description = "Signature Delegation" },
            new Workproject { Name = "DM1", Description = "Design Improvements & Tenders" },
            new Workproject { Name = "DM2", Description = "SKU design" },
            new Workproject { Name = "HR5", Description = "HR activities - Miscellanious" },
            new Workproject { Name = "PM1", Description = "PMDB Development" },
            new Workproject { Name = "PM2", Description = "Lean & Instructions/Manuals" },
            new Workproject { Name = "QM1", Description = "DDC & Quality Audits" },
            new Workproject { Name = "QM3", Description = "DDC Stress support" },
            new Workproject { Name = "SM1", Description = "Stress Improvements & Tenders" },
            new Workproject { Name = "SM2", Description = "SKU stress" },
            new Workproject { Name = "T07", Description = "Design Training preparation and conduct for trainers" },
            new Workproject { Name = "T08", Description = "Design On-job trainers" },
            new Workproject { Name = "T09", Description = "Stress Training preparation and conduct for trainers" },
            new Workproject { Name = "T10", Description = "Stress On-job trainers" },
            new Workproject { Name = "T50", Description = "Software development - trainings" }
        };

        private static Signatures[] Signatures =
        {
            new Signatures { ForObjectives = new ForObjectives(), ForResults = new ForResults() },
            new Signatures { ForObjectives = new ForObjectives(), ForResults = new ForResults() },
            new Signatures { ForObjectives = new ForObjectives(), ForResults = new ForResults() },
            new Signatures { ForObjectives = new ForObjectives(), ForResults = new ForResults() },
            new Signatures { ForObjectives = new ForObjectives(), ForResults = new ForResults() },
            new Signatures { ForObjectives = new ForObjectives(), ForResults = new ForResults() },
        };

        private static Definition[] Definitions =
        {
            new Definition { Employee = Users[1], Manager = Users[5], Approver = Users[7], Workproject = Workprojects[19], Period = Periods.Q3, Year = 2021 },
            new Definition { Employee = Users[2], Manager = Users[4], Approver = Users[6], Workproject = Workprojects[18], Period = Periods.Q1, Year = 2021 },
            new Definition { Employee = Users[3], Manager = Users[5], Approver = Users[7], Workproject = Workprojects[19], Period = Periods.Q4, Year = 2021 },
            new Definition { Employee = Users[0], Manager = Users[4], Approver = Users[6], Workproject = Workprojects[18], Period = Periods.Q1, Year = 2021 },

            new Definition { Employee = Users[4], Manager = Users[4], Approver = Users[6], Workproject = Workprojects[18], Period = Periods.Q1, Year = 2021, IsWpmHox = true },
            new Definition { Employee = Users[5], Manager = Users[5], Approver = Users[7], Workproject = Workprojects[19], Period = Periods.Q1, Year = 2021, IsWpmHox = true },
        };

        private static Conclusion[] Conclusions =
        {
            new Conclusion { },
            new Conclusion { },
            new Conclusion { },
            new Conclusion { },

            new Conclusion { },
            new Conclusion { },
        };

        private static Form[] Forms =
        {
            new Form { Definition = Definitions[0], Conclusion = Conclusions[0], Signatures = Signatures[0] },
            new Form { Definition = Definitions[1], Conclusion = Conclusions[1], Signatures = Signatures[1] },
            new Form { Definition = Definitions[2], Conclusion = Conclusions[2], Signatures = Signatures[2] },
            new Form { Definition = Definitions[3], Conclusion = Conclusions[3], Signatures = Signatures[3] },

            new Form { Definition = Definitions[4], Conclusion = Conclusions[4], Signatures = Signatures[4] },
            new Form { Definition = Definitions[5], Conclusion = Conclusions[5], Signatures = Signatures[5] },
        };

        private static ObjectiveResult[] ObjectivesResults =
        {
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Descrition #{1}", Unit = $"Some Unit #{1}", IsKey = true, IsMeasurable = true, Threshold = "60", Target = "80", Challenge = "100", WeightFactor = "20", KpiUpperLimit = "120" }, Form = Forms[0] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Descrition #{2}", Unit = $"Some Unit #{2}", IsKey = true, IsMeasurable = true, Threshold = "45", Target = "70", Challenge = "90", WeightFactor = "20", KpiUpperLimit = "117" }, Form = Forms[0] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Descrition #{3}", Unit = $"Some Unit #{3}", IsKey = true, IsMeasurable = false, Threshold = "80", Target = "60", Challenge = "30", WeightFactor = "15", KpiUpperLimit = "115" }, Form = Forms[0] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Descrition #{4}", Unit = $"Some Unit #{4}", IsKey = true, IsMeasurable = true, Threshold = "10", Target = "40", Challenge = "60", WeightFactor = "15", KpiUpperLimit = "120" }, Form = Forms[0] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Descrition #{5}", Unit = $"Some Unit #{5}", IsKey = true, IsMeasurable = false, Threshold = "N/A", Target = "", Challenge = "", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[0] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Descrition #{6}", Unit = $"Some Unit #{6}", IsKey = true, IsMeasurable = true, Threshold = "90", Target = "110", Challenge = "140", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[0] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Descrition #{7}", Unit = $"Some Unit #{7}", IsKey = false, IsMeasurable = false, Threshold = "75", Target = "N/A", Challenge = "N/A", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[0] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"Statement #{8}", Description = $"Descrition #{8}", Unit = $"Some Unit #{8}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[0] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"Statement #{9}", Description = $"Descrition #{9}", Unit = $"Some Unit #{9}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[0] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"Statement #{10}", Description = $"Descrition #{10}", Unit = $"Some Unit #{10}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[0] },

            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Descrition #{1}", Unit = $"Some Unit #{1}", IsKey = false, IsMeasurable = false, Threshold = "30", Target = "50", Challenge = "70", WeightFactor = "20", KpiUpperLimit = "115" }, Form = Forms[1] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Descrition #{2}", Unit = $"Some Unit #{2}", IsKey = true, IsMeasurable = true, Threshold = "60", Target = "70", Challenge = "110", WeightFactor = "20", KpiUpperLimit = "117" }, Form = Forms[1] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Descrition #{3}", Unit = $"Some Unit #{3}", IsKey = true, IsMeasurable = false, Threshold = "24", Target = "30", Challenge = "55", WeightFactor = "15", KpiUpperLimit = "115" }, Form = Forms[1] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Descrition #{4}", Unit = $"Some Unit #{4}", IsKey = true, IsMeasurable = true, Threshold = "80", Target = "60", Challenge = "35", WeightFactor = "15", KpiUpperLimit = "120" }, Form = Forms[1] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Descrition #{5}", Unit = $"Some Unit #{5}", IsKey = true, IsMeasurable = false, Threshold = "1", Target = "7", Challenge = "11", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[1] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Descrition #{6}", Unit = $"Some Unit #{6}", IsKey = true, IsMeasurable = true, Threshold = "33", Target = "54", Challenge = "67", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[1] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Descrition #{7}", Unit = $"Some Unit #{7}", IsKey = false, IsMeasurable = false, Threshold = "N/A", Target = "", Challenge = "", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[1] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"Statement #{8}", Description = $"Descrition #{8}", Unit = $"Some Unit #{8}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[1] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"Statement #{9}", Description = $"Descrition #{9}", Unit = $"Some Unit #{9}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[1] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"Statement #{10}", Description = $"Descrition #{10}", Unit = $"Some Unit #{10}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[1] },

            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Descrition #{1}", Unit = $"Some Unit #{1}", IsKey = true, IsMeasurable = false, Threshold = "45", Target = "55", Challenge = "65", WeightFactor = "20", KpiUpperLimit = "119" }, Form = Forms[2] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Descrition #{2}", Unit = $"Some Unit #{2}", IsKey = false, IsMeasurable = true, Threshold = "44", Target = "20", Challenge = "73", WeightFactor = "20", KpiUpperLimit = "117" }, Form = Forms[2] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Descrition #{3}", Unit = $"Some Unit #{3}", IsKey = true, IsMeasurable = true, Threshold = "99", Target = "102", Challenge = "110", WeightFactor = "15", KpiUpperLimit = "115" }, Form = Forms[2] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Descrition #{4}", Unit = $"Some Unit #{4}", IsKey = false, IsMeasurable = true, Threshold = "15", Target = "25", Challenge = "30", WeightFactor = "15", KpiUpperLimit = "120" }, Form = Forms[2] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Descrition #{5}", Unit = $"Some Unit #{5}", IsKey = false, IsMeasurable = false, Threshold = "90", Target = "", Challenge = "", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[2] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Descrition #{6}", Unit = $"Some Unit #{6}", IsKey = true, IsMeasurable = false, Threshold = "33", Target = "54", Challenge = "67", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[2] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Descrition #{7}", Unit = $"Some Unit #{7}", IsKey = false, IsMeasurable = false, Threshold = "N/A", Target = "", Challenge = "", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[2] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"Statement #{8}", Description = $"Descrition #{8}", Unit = $"Some Unit #{8}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[2] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"Statement #{9}", Description = $"Descrition #{9}", Unit = $"Some Unit #{9}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[2] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"Statement #{10}", Description = $"Descrition #{10}", Unit = $"Some Unit #{10}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[2] },

            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Descrition #{1}", Unit = $"Some Unit #{1}", IsKey = true, IsMeasurable = true, Threshold = "30", Target = "55", Challenge = "65", WeightFactor = "20", KpiUpperLimit = "119" }, Form = Forms[3] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Descrition #{2}", Unit = $"Some Unit #{2}", IsKey = true, IsMeasurable = false, Threshold = "44", Target = "20", Challenge = "73", WeightFactor = "20", KpiUpperLimit = "117" }, Form = Forms[3] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Descrition #{3}", Unit = $"Some Unit #{3}", IsKey = false, IsMeasurable = false, Threshold = "99", Target = "N/A", Challenge = "N/A", WeightFactor = "15", KpiUpperLimit = "115" }, Form = Forms[3] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Descrition #{4}", Unit = $"Some Unit #{4}", IsKey = true, IsMeasurable = true, Threshold = "44", Target = "60", Challenge = "75", WeightFactor = "15", KpiUpperLimit = "120" }, Form = Forms[3] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Descrition #{5}", Unit = $"Some Unit #{5}", IsKey = false, IsMeasurable = true, Threshold = "90", Target = "", Challenge = "", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[3] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Descrition #{6}", Unit = $"Some Unit #{6}", IsKey = true, IsMeasurable = false, Threshold = "25", Target = "55", Challenge = "67", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[3] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Descrition #{7}", Unit = $"Some Unit #{7}", IsKey = false, IsMeasurable = false, Threshold = "N/A", Target = "", Challenge = "", WeightFactor = "10", KpiUpperLimit = "115" }, Form = Forms[3] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"Statement #{8}", Description = $"Descrition #{8}", Unit = $"Some Unit #{8}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[3] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"Statement #{9}", Description = $"Descrition #{9}", Unit = $"Some Unit #{9}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[3] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"Statement #{10}", Description = $"Descrition #{10}", Unit = $"Some Unit #{10}", IsKey = true, IsMeasurable = true, Threshold = "", Target = "", Challenge = "", WeightFactor = "", KpiUpperLimit = "" }, Form = Forms[3] },
        };

        private static LocalAccess[] LocalAccess =
        {
            new LocalAccess { Form = Forms[1], User = Users[6] }
        };

        private static GlobalAccess[] GlobalAccess =
        {
            new GlobalAccess { User = Users[8], Department = Departments[2], Team = Teams[3] },
            new GlobalAccess { User = Users[8], Department = Departments[2], Team = Teams[5] },
            new GlobalAccess { User = Users[6], Department = Departments[2], Team = Teams[5] },
        };


        public static void Seed(IServiceProvider serviceProvider)
        {
            DataContext context = new DataContext(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>());

            if (context.Departments.Count() == 0)
                context.Departments.AddRange(Departments);

            if (context.Positions.Count() == 0)
                context.Positions.AddRange(Positions);

            if (context.Teams.Count() == 0)
                context.Teams.AddRange(Teams);

            if (context.Users.Count() == 0)
                context.Users.AddRange(Users);

            if (context.Workprojects.Count() == 0)
                context.Workprojects.AddRange(Workprojects);

            if (context.Definitions.Count() == 0)
                context.Definitions.AddRange(Definitions);

            if (context.Conclusions.Count() == 0)
                context.Conclusions.AddRange(Conclusions);

            if (context.Forms.Count() == 0)
                context.Forms.AddRange(Forms);

            if (context.ObjectivesResults.Count() == 0)
                context.ObjectivesResults.AddRange(ObjectivesResults);

            if (context.LocalAccess.Count() == 0)
                context.LocalAccess.AddRange(LocalAccess);

            if (context.GlobalAccess.Count() == 0)
                context.GlobalAccess.AddRange(GlobalAccess);

            context.SaveChanges();
        }
    }
}
