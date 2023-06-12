using BonusSystemApplication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            new Position { NameEng = "General Director", NameRus = "Генеральный Директор", Abbreviation = "GDR" },
            new Position { NameEng = "Design Engineer - Category X", NameRus = "Инженер-конструктор X категории", Abbreviation = "DECX" },
            new Position { NameEng = "Lead Design Engineer", NameRus = "Старший инженер инженер-конструктор", Abbreviation = "LDE" },
            new Position { NameEng = "Project Leader (Design)", NameRus = "Руководитель проекта (дизайн)", Abbreviation = "DPL" },
            new Position { NameEng = "Project Leader (Stress)", NameRus = "Руководитель проекта (прочность)", Abbreviation = "SPL" },
            new Position { NameEng = "Lead Stress Engineer", NameRus = "Ведущий инженер по прочности", Abbreviation = "SLE" },
            new Position { NameEng = "Senior Stress Engineer", NameRus = "Старший инженер по прочности", Abbreviation = "SSE" },
            new Position { NameEng = "Head of Design", NameRus = "Руководитель конструкторского направления", Abbreviation = "HOD" },
            new Position { NameEng = "Head of Stress", NameRus = "Руководитель направления прочности", Abbreviation = "HOS" },
            new Position { NameEng = "Head of HR", NameRus = "Директор по персоналу", Abbreviation = "HRD" },
            new Position { NameEng = "HR Specialist, Recruitment, Assessment and Motivation", NameRus = "Специалист по набору, оценке и мотивации персонала", Abbreviation = "HRR" },
            new Position { NameEng = "IT Manager", NameRus = "Менеджер по информационным технологиям", Abbreviation = "ITM" },
            new Position { NameEng = "WINDOWS Infrastructure Technical Support Specialist", NameRus = "Специалист по технической поддержке WINDOWS инфраструктуры", Abbreviation = "ITWS" },
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
            // Design & Stress
            //designers:
            new User { FirstNameEng = "Ivan", LastNameEng = "Ivanov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Iviva00", Email = "ivan.ivanov@domain.com", Pid = "dsg001",
                       Department = Departments[2], Team = Teams[3], Position = Positions[2]},
            new User { FirstNameEng = "Dmitriy", LastNameEng = "Dmitriev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Dmdmi00", Email = "dmitry.dmitriev@domain.com", Pid = "dsg002",
                       Department = Departments[2], Team = Teams[3], Position = Positions[3]},
            new User { FirstNameEng = "Sergey", LastNameEng = "Sergeev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Seser00", Email = "sergey.sergeev@domain.com", Pid = "dsg003",
                       Department = Departments[2], Team = Teams[3], Position = Positions[2]},
            new User { FirstNameEng = "Marat", LastNameEng = "Maratov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Mamar00", Email = "marat.maratov@domain.com", Pid = "dsg004",
                       Department = Departments[2], Team = Teams[5], Position = Positions[6]},
            new User { FirstNameEng = "Konstantin", LastNameEng = "Konstantinov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Kokon00", Email = "konstantin.konstantinov@domain.com", Pid = "dsg005",
                       Department = Departments[2], Team = Teams[5], Position = Positions[7]},
            new User { FirstNameEng = "Vladimir", LastNameEng = "Vladimirov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Vlvla00", Email = "vladimir.vladimirov@domain.com", Pid = "dsg006",
                       Department = Departments[2], Team = Teams[5], Position = Positions[6]},

            //managers:
            new User { FirstNameEng = "Karl", LastNameEng = "Karlov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Kakar00", Email = "karl.karlov@domain.com", Pid = "mng001",
                       Department = Departments[2], Team = Teams[3], Position = Positions[4]},
            new User { FirstNameEng = "Alexander", LastNameEng = "Alexandrov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Alale00", Email = "alexander.alexadrov@domain.com", Pid = "mng002",
                       Department = Departments[2], Team = Teams[5], Position = Positions[5]},

            //approvers:
            new User { FirstNameEng = "Petr", LastNameEng = "Petrov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Pepet00", Email = "petr.petrov@domain.com", Pid = "apr001",
                       Department = Departments[2], Team = Teams[3], Position = Positions[8]},
            new User { FirstNameEng = "Mark", LastNameEng = "Markov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Mamar01", Email = "marl.markov@domain.com", Pid = "apr002",
                       Department = Departments[2], Team = Teams[5], Position = Positions[9]},

            // IT
            new User { FirstNameEng = "Viktor", LastNameEng = "Viktorov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Vivik00", Email = "viktor.viktorov@domain.com", Pid = "it001",
                       Department = Departments[5], Team = Teams[0], Position = Positions[13]},
            new User { FirstNameEng = "Alexey", LastNameEng = "Alexeev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "AlAle01", Email = "Alexey.Alexeev@domain.com", Pid = "mng003",
                       Department = Departments[5], Team = Teams[0], Position = Positions[13]},
            new User { FirstNameEng = "Tikhon", LastNameEng = "Tikhonov", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Titik00", Email = "tikhon.tikhonov@domain.com", Pid = "apr003",
                       Department = Departments[5], Team = Teams[0], Position = Positions[12]},

            // HR
            new User { FirstNameEng = "Nikolay", LastNameEng = "Nikolaev", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Ninik00", Email = "nikolay.nikolayev@domain.com", Pid = "hr001",
                       Department = Departments[4], Team = Teams[0], Position = Positions[10]},
            new User { FirstNameEng = "Anna", LastNameEng = "Annova", FirstNameRus = "", LastNameRus = "", MiddleNameRus = "", Login = "Anann00", Email = "anna.annova@domain.com", Pid = "hr002",
                       Department = Departments[4], Team = Teams[0], Position = Positions[11]},
        };

        private static Workproject[] Workprojects =
                {
            new Workproject { Name = "All", Description = "All WP (including N/A)" },
            new Workproject { Name = "101", Description = "Serial Activity" },
            new Workproject { Name = "112", Description = "Serial Activity Stress - AD Static" },
            new Workproject { Name = "118", Description = "SA/LR/A380 Mechanical Systems Installation" },
            new Workproject { Name = "AM9", Description = "Automation" },
            new Workproject { Name = "HR7", Description = "HR activities" },

        };

        private static Signatures[] Signatures =
        {
            new Signatures { },
            new Signatures { },
            new Signatures { },
            new Signatures { },
            new Signatures { },
            new Signatures { },
        };

        private static Definition[] Definitions =
        {
            new Definition { Employee = Users[0], Manager = Users[6], Approver = Users[8], Workproject = Workprojects[1], Period = Periods.Q1, Year = 2023 },
            new Definition { Employee = Users[1], Manager = Users[6], Approver = Users[8], Workproject = Workprojects[3], Period = Periods.Q1, Year = 2023 },
            new Definition { Employee = Users[3], Manager = Users[7], Approver = Users[9], Workproject = Workprojects[2], Period = Periods.Q1, Year = 2023 },
            new Definition { Employee = Users[4], Manager = Users[7], Approver = Users[9], Workproject = Workprojects[2], Period = Periods.Q1, Year = 2023 },
            new Definition { Employee = Users[10], Manager = Users[11], Approver = Users[12], Workproject = Workprojects[4], Period = Periods.S1, Year = 2023, IsWpmHox = true },
            new Definition { Employee = Users[14], Manager = Users[13], Approver = Users[13], Workproject = Workprojects[5], Period = Periods.Y, Year = 2023, IsWpmHox = true },
        };

        private static Conclusion[] Conclusions =
        {
            new Conclusion { },
            new Conclusion { },
            new Conclusion { ManagerComment = "Item 6.3. Inapropriate filling 1C and PMDB on daily basis was registered." },
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
            // form[0]
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Earn vs Spent (Productivity)", Description = $"Hours spent by the design team (FP) covered by provided deliveries. Earned vs to Spent Ratio.",
                                  Unit = $"ratio, %", IsKey = true, IsMeasurable = true, Threshold = 95, Target = 110, Challenge = 125, WeightFactor = 25, KpiUpperLimit = 120 }, Form = Forms[0] },

            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Design", Description = $"1. Develop technical skills of the design team. To provide adequate feedback to low performers.\r\n" +
                                                                                                            $"2. Planning and controlling of deliverables: highlighted in advance all blocking issues;\r\n" +
                                                                                                            $"3. Planning and controlling of resourses. Participation in Resource diagram regular update (bi-weekly basis);\r\n" +
                                                                                                            $"4. To Develop Task calculator for Belly Fairing perimeter\r\n" +
                                                                                                            $"5. Independent management of FP perimeter\r\n" +
                                                                                                            $"6. To prepare all design perimeters for Q6 assessement\r\n" +
                                                                                                            $"7. To use task calc catalog for FP perimeter",
                                  Unit = $"items done", IsKey = true, IsMeasurable = true, Threshold = 5, Target = 6, Challenge = 7, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[0] },

            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Internal Quality (Formal)", Description = $"Internal DDC Right First Time in Sheets. No repetitive mistakes are allowed.",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 80, Target = 90, Challenge = 95, WeightFactor = 20, KpiUpperLimit = 110 }, Form = Forms[0] },

            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Manager's feedback", Description = $"Criteria: Direct Manager's Feedback which takes into account overall intensity of the work, involvement into the processes, reactivity and pro-activity, professional behavior, team spirit and fulfilment of basic ECAR procedure. \r\n" +
                                                                                                                        $"Excellent = 115%, Normal = 100%, Need to improve <100%",
                                  Unit = $"KPI value, %", IsKey = true, IsMeasurable = false, Threshold = 80, Target = null, Challenge = null, WeightFactor = 10, KpiUpperLimit = 115 }, Form = Forms[0] },

            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Technical Quality", Description = $"Airbus and ECAR Technical Quality Right First Time",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 95, Target = 98, Challenge = 100, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[0] },

            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Improvements", Description = $"To create neither a several simple scripts to simplify a work nor one complex macros (for GSuite for example)",
                                  Unit = $"Week number", IsKey = true, IsMeasurable = true, Threshold = 14, Target = 12, Challenge = 10, WeightFactor = 10, KpiUpperLimit = 110 }, Form = Forms[0] },

            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"On-time delivery", Description = $"Average delay vs. agreed target days. IT issues will not be considered.",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 95, Target = 98, Challenge = 100, WeightFactor = 5, KpiUpperLimit = 108 }, Form = Forms[0] },

            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"PMDB rules", Description = $"1. PMDB WP and 1C completion on daily basis. Working hours fulfillment; \r\n" +
                                                                                                                $"2. All required signatures provided on-time; \r\n" +
                                                                                                                $"3. PMDB always match to 1C, personal and team;\r\n" +
                                                                                                                $"4. Avoid integral tasks. \r\n" +
                                                                                                                $"5. Avoid mistakes in New PMDB. (wrong template, UFP, MSN)\r\n" +
                                                                                                                $"6. Status monitoring in PMDB: keep up-to-date;",
                                  Unit = $"Number of reached", IsKey = true, IsMeasurable = true, Threshold = 4, Target = 5, Challenge = 6, WeightFactor = 0, KpiUpperLimit = 120 }, Form = Forms[0] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[0] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[0] },



            // form[1]
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Earn vs Spent (Productivity)", Description = $"To be in line with the hours appointed in PMDB by design leader.\r\nIf assigned hours exceeded, inform DL immediately",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 90, Target = 105, Challenge = 115, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[1] },

            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"External Quality (Technical)", Description = $"Airbus Technical Right First Time. Official reject or repetitive mistakes from Airbus quality reports shall be considered as a blocking point! Rejects from ECAR technical checker to be taken into account as well.",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 95, Target = 98, Challenge = 100, WeightFactor = 15, KpiUpperLimit = 115 }, Form = Forms[1] },

            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Internal Quality (Formal)", Description = $"Internal DDC Right First Time in Sheets. No repetitive mistakes are allowed.\r\nIn case of no DDC deliverables, the item will be considered as a target",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 80, Target = 90, Challenge = 95, WeightFactor = 15, KpiUpperLimit = 115 }, Form = Forms[1] },

            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"On time Delivery", Description = $"KPI Value based on CLDB data or Milestones from DL's schedule. Measured on CLDB Checklist data storage compared with ADB/RQS data required.",
                                  Unit = $"%", IsKey = true, IsMeasurable = true, Threshold = 95, Target = 98, Challenge = 100, WeightFactor = 15, KpiUpperLimit = 110 }, Form = Forms[1] },

            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Manager's feedback", Description = $"Criteria: Direct Manager's Feedback which takes into account overall intensity of the work, involvement into the processes, reactivity and pro-activity, professional behavior, team spirit and fulfilment of basic ECAR procedure; to pass E&C training. Confirmation sheet should be signed within one week the request received.\r\nExcellent = 110%, Satisfactory = 100%, Need to improve <100%",
                                  Unit = $"KPI value, %", IsKey = true, IsMeasurable = false, Threshold = 85, Target = null, Challenge = null, WeightFactor = 15, KpiUpperLimit = 110 }, Form = Forms[1] },

            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Project Needs", Description = $"1. To learn WP way of working (Task tracking, sourses for documentation)\r\n" +
                                                                                                                   $"2. Communication with cusromer per mail\r\n" +
                                                                                                                   $"3. XLR MOD 1/2/3 + Brackets support (3d/2d, releasing)\r\n" +
                                                                                                                   $"4. Topic7: UWF → Support (3d/2d, releasing)\r\n" +
                                                                                                                   $"5. DP's , I/F  preparation support\r\n" +
                                                                                                                   $"6. To learn and use updated internal Check lists (Tech. / Formal)\r\n" +
                                                                                                                   $"Excellent = 115%, Good = 100%, Need to improve <100%  ",
                                  Unit = $"N of actions done", IsKey = true, IsMeasurable = true, Threshold = 4, Target = 5, Challenge = 6, WeightFactor = 15, KpiUpperLimit = 120 }, Form = Forms[1] },

            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"PMDB rules", Description = $"1. PMDB WP and 1C completion on daily basis. Working hours fulfillment\r\n" +
                                                                                                                $"2. PMDB always match to 1C, personal.\r\n" +
                                                                                                                $"3. Inform DL in case of overspending hours\r\n" +
                                                                                                                $"4. PMDB C/L closure: Author & performer names are different (four eyes principle)\r\n" +
                                                                                                                $"5. Status DONE for each finished task is present in PMDB ",
                                  Unit = $"Number of reached targets", IsKey = true, IsMeasurable = true, Threshold = 3, Target = 4, Challenge = 5, WeightFactor = 5, KpiUpperLimit = 108 }, Form = Forms[1] },

            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[1] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[1] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[1] },



            // form[2]
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Earn vs Spent (Productivity)", Description = $"Earn / spent ratio is calculated as the hours assigned by stress leader / hours spent in accordance with PMDB. Responsibility for the entire Easy Fit Brackets perimeter.",
                                  Unit = $"ratio, %", IsKey = true, IsMeasurable = true, Threshold = 95, Target = 100, Challenge = 115, WeightFactor = 25, KpiUpperLimit = 118 }, Form = Forms[2] },

            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"On-time delivery", Description = $"Personal KPI based on PMDB data with task end. Calculated as ratio between number of closed in time tasks / total number of tasks (In case of number of tasks > 10)",
                                  Unit = $"KPI value, %", IsKey = true, IsMeasurable = true, Threshold = 85, Target = 96, Challenge = 100, WeightFactor = 25, KpiUpperLimit = 108 }, Form = Forms[2] },

            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Internal Quality", Description = $"Quality of stress deliverables (calculations, reports and so on).\r\n" +
                                                                                   $"KPI base on PMDB data.",
                                  Unit = $"KPI value, %", IsKey = true, IsMeasurable = true, Threshold = 80, Target = 95, Challenge = 100, WeightFactor = 20, KpiUpperLimit = 115 }, Form = Forms[2] },

            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Direct manager's feedback", Description = $"Criteria: Direct Manager's Feedback which takes into account overall intensity of the work, involvement into the processes, flexibility, reactivity and pro-activity, professional behavior, team spirit and fulfilment of basic ECAR procedure. \r\n" +
                                                                                                                               $"115% - Several stress project improvments external/internal to be proposed (confirmed by Airbus colleagues or Stress leader) and no any complains from customer about work within given perimeter\r\n" +
                                                                                                                               $"100% - Just no any complains from customer about work within given perimeter and normal way of working\r\n" +
                                                                                                                               $"80% - 100% - Minor deviations; <80% - exist sever complaints",
                                  Unit = $"KPI value, %", IsKey = true, IsMeasurable = false, Threshold = 80, Target = null, Challenge = null, WeightFactor = 20, KpiUpperLimit = 115 }, Form = Forms[2] },

            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Permanent Improve Efficiency", Description = $"Independent management of individual perimeter (to defined by SL, but preliminary - Easy Fit brackets), including communication with Customer concerning all the questions. Planning and controlling of deliverables: highlighted in advance all blocking issues. PM/SL/DL feedbacks to be taken into account as follow: \r\n" +
                                                                                                                                  $"115% - Excellent deedback from the Customers (Airbus, ECAR). Brilliant overal loerational results within given perimeter;\r\n" +
                                                                                                                                  $"100% - Just no any complains from customers about work within given perimeter and normal way of working\r\n" +
                                                                                                                                  $"80% - 100% - Minor deviations; <80% - exist sever complaints",
                                  Unit = $"KPI value, %", IsKey = true, IsMeasurable = false, Threshold = 85, Target = null, Challenge = null, WeightFactor = 10, KpiUpperLimit = 115 }, Form = Forms[2] },

            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"PMDB rules", Description = $"1. To strictly follow PMDB rules (example all done tasks should be closed);\r\n" +
                                                                                                                $"2. Planning and controlling of deliverables: highlighted in advance all blocking issues with missing inputs, stress validation or resources required;\r\n" +
                                                                                                                $"3. Fill 1C and PMDB on daily basis;\r\n" +
                                                                                                                $"4. Strictly follow project file storage rules;",
                                  Unit = $"Number of items", IsKey = true, IsMeasurable = true, Threshold = 2, Target = 3, Challenge = 4, WeightFactor = 0, KpiUpperLimit = 108 }, Form = Forms[2] },

            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = 10, KpiUpperLimit = 115 }, Form = Forms[2] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[2] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[2] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[2] },

            // form[3]
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Description #{1}", Unit = $"Unit #{1}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 30, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Description #{2}", Unit = $"Unit #{2}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Description #{3}", Unit = $"Unit #{3}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Description #{4}", Unit = $"Unit #{4}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Description #{5}", Unit = $"Unit #{5}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Description #{6}", Unit = $"Unit #{6}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 5, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Description #{7}", Unit = $"Unit #{7}", IsKey = true, IsMeasurable = true, Threshold = 30, Target = 55, Challenge = 65, WeightFactor = 5, KpiUpperLimit = 120 }, Form = Forms[3] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[3] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[3] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[3] },
        
        
            // form[4]
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Description #{1}", Unit = $"Unit #{1}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 30, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Description #{2}", Unit = $"Unit #{2}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Description #{3}", Unit = $"Unit #{3}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Description #{4}", Unit = $"Unit #{4}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Description #{5}", Unit = $"Unit #{5}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Description #{6}", Unit = $"Unit #{6}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 5, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Description #{7}", Unit = $"Unit #{7}", IsKey = true, IsMeasurable = true, Threshold = 15, Target = 35, Challenge = 99, WeightFactor = 5, KpiUpperLimit = 120 }, Form = Forms[4] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[4] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[4] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[4] },
        
        
            // form[5]
            new ObjectiveResult { Row = 1, Objective = new Objective { Statement = $"Statement #{1}", Description = $"Description #{1}", Unit = $"Unit #{1}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 30, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 2, Objective = new Objective { Statement = $"Statement #{2}", Description = $"Description #{2}", Unit = $"Unit #{2}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 3, Objective = new Objective { Statement = $"Statement #{3}", Description = $"Description #{3}", Unit = $"Unit #{3}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 20, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 4, Objective = new Objective { Statement = $"Statement #{4}", Description = $"Description #{4}", Unit = $"Unit #{4}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 5, Objective = new Objective { Statement = $"Statement #{5}", Description = $"Description #{5}", Unit = $"Unit #{5}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 10, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 6, Objective = new Objective { Statement = $"Statement #{6}", Description = $"Description #{6}", Unit = $"Unit #{6}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 5, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 7, Objective = new Objective { Statement = $"Statement #{7}", Description = $"Description #{7}", Unit = $"Unit #{7}", IsKey = true, IsMeasurable = true, Threshold = 64, Target = 87, Challenge = 110, WeightFactor = 5, KpiUpperLimit = 120 }, Form = Forms[5] },
            new ObjectiveResult { Row = 8, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[5] },
            new ObjectiveResult { Row = 9, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[5] },
            new ObjectiveResult { Row = 10, Objective = new Objective { Statement = $"", Description = $"", Unit = $"", IsKey = true, IsMeasurable = true, Threshold = null, Target = null, Challenge = null, WeightFactor = null, KpiUpperLimit = null }, Form = Forms[5] },
        };

        private static LocalAccess[] LocalAccess =
        {
            new LocalAccess { User = Users[13], Form = Forms[4] }
        };

        private static GlobalAccess[] GlobalAccess =
        {
            new GlobalAccess { User = Users[13], Department = Departments[2], Team = null },
            new GlobalAccess { User = Users[12], Department = Departments[5], Team = null },
            new GlobalAccess { User = Users[8], Department = Departments[2], Team = Teams[3] },
            new GlobalAccess { User = Users[9], Department = Departments[2], Team = Teams[5] },
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
