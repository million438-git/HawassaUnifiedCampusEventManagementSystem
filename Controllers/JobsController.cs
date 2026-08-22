using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HawassaUnifiedCampusEventManagementSystem.Data;
using HawassaUnifiedCampusEventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<JobsController> _logger;

        // In-memory student applications store for live demo and testing
        private static readonly List<StudentApplicationItemViewModel> SubmittedApplications = new()
        {
            new StudentApplicationItemViewModel
            {
                ApplicationId = "APP-HU-2026-0811",
                JobId = 1,
                JobTitle = "Software Developer Intern",
                CompanyName = "ABC Technology",
                Location = "Hawassa, Ethiopia",
                JobType = "Internship",
                AppliedAt = DateTime.UtcNow.AddDays(-3),
                Status = "Under Review",
                StatusBadgeClass = "bg-warning-subtle text-warning-emphasis",
                Notes = "Resume forwarded to Lead Engineer. Technical assessment invitation pending."
            },
            new StudentApplicationItemViewModel
            {
                ApplicationId = "APP-HU-2026-0742",
                JobId = 2,
                JobTitle = "Junior Network Engineer",
                CompanyName = "Ethio Telecom",
                Location = "Addis Ababa, Ethiopia",
                JobType = "Full-Time",
                AppliedAt = DateTime.UtcNow.AddDays(-7),
                Status = "Shortlisted",
                StatusBadgeClass = "bg-success-subtle text-success-emphasis",
                Notes = "Shortlisted for written exam at Hawassa Regional Center."
            }
        };

        // Static Master Catalog of 24 High-Quality Hawassa & Ethiopian Jobs
        private static readonly List<JobPostingViewModel> MasterJobsCatalog = new()
        {
            new JobPostingViewModel
            {
                Id = 1,
                Title = "Software Developer Intern",
                Slug = "software-developer-intern-abc-technology",
                CompanyName = "ABC Technology",
                CompanyInitials = "ABC",
                CompanyColor = "#4f46e5",
                Industry = "Software & IT",
                JobType = "Internship",
                WorkplaceType = "On-site",
                Location = "Hawassa, Ethiopia",
                CampusLocation = "Hawassa City / Near IoT Campus",
                ShortDescription = "Learn software development and work with an experienced development team on real client applications.",
                Description = "ABC Technology is seeking enthusiastic 3rd and 4th-year Computer Science and Software Engineering students from Hawassa University for our 3-month paid summer/semester internship. You will collaborate with senior developers on web and mobile systems, participate in agile standups, and write clean, scalable code in modern frameworks.",
                Requirements = new List<string>
                {
                    "Current 3rd or 4th year student in Computer Science, Software Engineering, or IT at Hawassa University",
                    "Familiarity with C#, ASP.NET Core, Java, or JavaScript/TypeScript",
                    "Basic understanding of SQL databases and Git version control",
                    "Strong problem-solving skills and eagerness to learn in a fast-paced environment",
                    "Minimum cumulative GPA of 3.0 preferred"
                },
                Responsibilities = new List<string>
                {
                    "Develop and maintain web API endpoints and responsive user interface components",
                    "Participate in daily agile sprint rituals, code reviews, and pair programming sessions",
                    "Write unit tests and assist with bug fixing and performance profiling",
                    "Document technical specifications and system architectures alongside mentors"
                },
                Skills = new List<string> { "C#", ".NET Core", "JavaScript", "SQL", "Git", "REST APIs" },
                SalaryDisplay = "ETB 8,500 / month (Stipend)",
                Deadline = new DateTime(2026, 8, 30),
                IsClosingSoon = true,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 38,
                ViewsCount = 312,
                Eligibility = "3rd & 4th Year Computer Science / Software Engineering Students",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "careers@abctechnology.et",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            new JobPostingViewModel
            {
                Id = 2,
                Title = "Junior Network Engineer",
                Slug = "junior-network-engineer-ethio-telecom",
                CompanyName = "Ethio Telecom",
                CompanyInitials = "ET",
                CompanyColor = "#008751",
                Industry = "Telecommunications",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Addis Ababa, Ethiopia",
                CampusLocation = "Southern Region & Head Office Rotations",
                ShortDescription = "Work with nationwide network infrastructure and modern telecommunication systems.",
                Description = "Ethio Telecom is recruiting talented fresh graduates and junior engineers to join our Southern Regional Directorate and Central Network Operations Center. You will monitor optical networks, troubleshoot routing and switching hardware, configure enterprise VPNs, and support the expansion of 4G/5G mobile services across the nation.",
                Requirements = new List<string>
                {
                    "B.Sc. in Electrical & Computer Engineering, Computer Science, or Network Engineering",
                    "Solid knowledge of TCP/IP networking, Cisco iOS, BGP, OSPF, and VLAN architectures",
                    "CCNA / CompTIA Network+ certification or university coursework equivalent is a plus",
                    "Willingness to travel for field deployments and system maintenance across regional sites",
                    "Good English and Amharic communication skills"
                },
                Responsibilities = new List<string>
                {
                    "Monitor network availability, packet latency, and uptime 24/7 across telecom hubs",
                    "Configure routers, switches, firewalls, and microwave transmission hardware",
                    "Investigate network outages and execute fast incident recovery protocols",
                    "Collaborate with field technicians on fiber-optic splicing and site commissions"
                },
                Skills = new List<string> { "Cisco", "TCP/IP", "Routing & Switching", "Fiber Optics", "Network Security", "Linux" },
                SalaryDisplay = "ETB 18,500 - 24,000 / month + Benefits",
                Deadline = new DateTime(2026, 9, 10),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 74,
                ViewsCount = 580,
                Eligibility = "Graduating Seniors & Recent Alumni (within 2 years)",
                ExperienceLevel = "Fresh Graduate / Entry Level",
                ApplicationEmail = "recruitment@ethiotelecom.et",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new JobPostingViewModel
            {
                Id = 3,
                Title = "Frontend Web Developer (React / UI)",
                Slug = "frontend-web-developer-hawassa-tech-hub",
                CompanyName = "Hawassa Tech Innovation Hub",
                CompanyInitials = "HTH",
                CompanyColor = "#0284c7",
                Industry = "Software & IT",
                JobType = "Full-Time",
                WorkplaceType = "Hybrid",
                Location = "Hawassa, Ethiopia",
                CampusLocation = "Hawassa University IoT Incubator",
                ShortDescription = "Build modern, accessible web applications for campus startups and regional businesses.",
                Description = "Join the Hawassa Tech Innovation Hub at the Institute of Technology (IoT) campus. We build digital products for fintech, education, and health tech clients. We are looking for a creative Frontend Web Developer who loves creating pixel-perfect, snappy web user experiences using React, TypeScript, and Tailwind CSS.",
                Requirements = new List<string>
                {
                    "Degree or senior student status in Computer Science, Software Engineering, or related discipline",
                    "Proven hands-on experience building web applications with React or Next.js",
                    "Strong knowledge of modern CSS, responsive layouts, flexbox/grid, and web accessibility",
                    "Experience consuming RESTful JSON APIs and managing component state",
                    "A link to your GitHub profile or live project portfolio is required"
                },
                Responsibilities = new List<string>
                {
                    "Translate Figma design mocks into clean, interactive, high-performance web components",
                    "Optimize web performance, Core Web Vitals, and mobile responsiveness",
                    "Work directly with backend engineers to integrate REST and GraphQL endpoints",
                    "Mentor junior student developers participating in tech incubator cohorts"
                },
                Skills = new List<string> { "React.js", "TypeScript", "Tailwind CSS", "HTML5/CSS3", "Figma", "Redux" },
                SalaryDisplay = "ETB 16,000 - 22,000 / month",
                Deadline = new DateTime(2026, 9, 15),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 42,
                ViewsCount = 390,
                Eligibility = "All Hawassa Students & Developers",
                ExperienceLevel = "Junior / Mid-Level",
                ApplicationEmail = "jobs@hawassahub.et",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },

            new JobPostingViewModel
            {
                Id = 4,
                Title = "Mobile App Developer Intern (Flutter / Dart)",
                Slug = "mobile-app-developer-intern-safaricom-ethiopia",
                CompanyName = "Safaricom Ethiopia",
                CompanyInitials = "SAF",
                CompanyColor = "#e11d48",
                Industry = "Telecommunications & FinTech",
                JobType = "Internship",
                WorkplaceType = "Hybrid",
                Location = "Hawassa & Addis Ababa, Ethiopia",
                CampusLocation = "Hawassa University Regional Office",
                ShortDescription = "Create innovative M-PESA and consumer mobile apps for millions of Ethiopian users.",
                Description = "Safaricom Ethiopia is looking for ambitious mobile application development interns to join our digital solutions engineering wing. You will help build and test mobile features for customer self-service, merchant payment solutions, and loyalty programs.",
                Requirements = new List<string>
                {
                    "Undergraduate student in Software Engineering, Computer Science, or Electrical Engineering",
                    "Experience developing cross-platform mobile apps with Flutter / Dart or React Native",
                    "Familiarity with mobile UI guidelines, offline caching, and REST integrations",
                    "Strong collaborative spirit and communication abilities"
                },
                Responsibilities = new List<string>
                {
                    "Implement smooth mobile UI screens and animations in Flutter",
                    "Integrate authentication, geolocation, and payment gateway APIs",
                    "Perform testing across Android and iOS device matrices"
                },
                Skills = new List<string> { "Flutter", "Dart", "Android", "iOS", "Firebase", "State Management" },
                SalaryDisplay = "ETB 11,000 / month (Paid Internship)",
                Deadline = new DateTime(2026, 9, 05),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 65,
                ViewsCount = 610,
                Eligibility = "3rd - 5th Year Engineering Students",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "graduates@safaricom.et",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new JobPostingViewModel
            {
                Id = 5,
                Title = "Data Analyst & Database Assistant",
                Slug = "data-analyst-database-assistant-cbe",
                CompanyName = "Commercial Bank of Ethiopia (CBE)",
                CompanyInitials = "CBE",
                CompanyColor = "#9333ea",
                Industry = "Banking & Finance",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Branch, Sidama",
                CampusLocation = "Hawassa Central Business District",
                ShortDescription = "Analyze financial transaction data, generate branch metrics, and maintain database integrity.",
                Description = "The Commercial Bank of Ethiopia is hiring a Junior Data Analyst for our Hawassa District Office. You will query large relational datasets, build PowerBI reporting dashboards, support audit verifications, and assist branch managers with operational metrics.",
                Requirements = new List<string>
                {
                    "B.Sc. in Computer Science, Information Systems, Statistics, or Economics",
                    "High proficiency with SQL (PostgreSQL, Oracle, or SQL Server) and Excel VBA",
                    "Experience with Power BI, Tableau, or Python data libraries (Pandas, Matplotlib)",
                    "High degree of integrity and attention to detail when dealing with sensitive data"
                },
                Responsibilities = new List<string>
                {
                    "Write optimized SQL queries and stored procedures for reporting pipelines",
                    "Create automated weekly executive dashboards and branch KPIs",
                    "Perform data cleaning and anomaly detection on financial records"
                },
                Skills = new List<string> { "SQL", "Power BI", "Excel Advanced", "Python Data", "Data Modeling" },
                SalaryDisplay = "ETB 17,500 - 23,000 / month + Banking Perks",
                Deadline = new DateTime(2026, 9, 25),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 89,
                ViewsCount = 720,
                Eligibility = "Graduates in CS, IS, Statistics, or Finance",
                ExperienceLevel = "Fresh Graduate",
                ApplicationEmail = "jobs@cbe.com.et",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },

            new JobPostingViewModel
            {
                Id = 6,
                Title = "Campus IT Support Specialist",
                Slug = "campus-it-support-specialist-hu-ict",
                CompanyName = "Hawassa University ICT Directorate",
                CompanyInitials = "HUICT",
                CompanyColor = "#16a34a",
                Industry = "Education & ICT",
                JobType = "Part-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Main Campus",
                CampusLocation = "Main Campus ICT Center & Computer Labs",
                ShortDescription = "Support campus computer labs, university Wi-Fi, faculty systems, and student portals.",
                Description = "Work right on campus with the Hawassa University ICT Directorate! We have flexible part-time positions tailored for current university students. Provide technical support to faculty members, troubleshoot lab PC installations, assist with campus Wi-Fi configuration, and maintain smart classrooms.",
                Requirements = new List<string>
                {
                    "Active Hawassa University student in good academic standing (2nd, 3rd, or 4th year)",
                    "Knowledge of Windows OS, Linux, basic networking, printer setup, and hardware maintenance",
                    "Friendly, helpful attitude and patience when helping professors and fellow students",
                    "Ability to commit 15-20 hours per week around your academic schedule"
                },
                Responsibilities = new List<string>
                {
                    "Diagnose and resolve hardware and software issues across campus computer laboratories",
                    "Assist students with institutional email activation and portal login difficulties",
                    "Set up audio-visual equipment and projectors for campus conferences and lectures",
                    "Log service tickets in the HUCEMS IT Helpdesk system"
                },
                Skills = new List<string> { "Hardware Troubleshooting", "Windows/Linux", "Wi-Fi Config", "Helpdesk", "Customer Support" },
                SalaryDisplay = "ETB 5,500 / month (Flexible Campus Role)",
                Deadline = new DateTime(2026, 8, 28),
                IsClosingSoon = true,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 51,
                ViewsCount = 430,
                Eligibility = "Current Hawassa University Students",
                ExperienceLevel = "Student Role",
                ApplicationEmail = "ict-support@hu.edu.et",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },

            new JobPostingViewModel
            {
                Id = 7,
                Title = "Cybersecurity Trainee & Analyst",
                Slug = "cybersecurity-trainee-analyst-insa",
                CompanyName = "INSA Ethiopia",
                CompanyInitials = "INSA",
                CompanyColor = "#0f172a",
                Industry = "Cybersecurity & Defense",
                JobType = "Internship",
                WorkplaceType = "On-site",
                Location = "Addis Ababa, Ethiopia",
                CampusLocation = "Cyber Security Operations Center",
                ShortDescription = "Train in threat detection, ethical hacking, vulnerability assessments, and defense systems.",
                Description = "The Information Network Security Administration (INSA) welcomes top Hawassa University computer science and engineering students to join our elite cybersecurity trainee fellowship. Learn threat intelligence, SOC monitoring, penetration testing techniques, and national infrastructure defense.",
                Requirements = new List<string>
                {
                    "Top 10% academic standing in Computer Science, Software Engineering, or Cybersecurity",
                    "Foundational understanding of network protocols, Linux administration, and cryptography",
                    "Experience with security tools like Wireshark, Nmap, Burp Suite, or Kali Linux is advantageous",
                    "Ethiopian citizenship and successful completion of security screening"
                },
                Responsibilities = new List<string>
                {
                    "Analyze security logs, SIEM alerts, and suspicious traffic patterns",
                    "Assist senior analysts in performing web application security assessments",
                    "Prepare technical writeups on newly discovered CVE vulnerabilities"
                },
                Skills = new List<string> { "Ethical Hacking", "Wireshark", "Linux Security", "SIEM", "Cryptography", "Python" },
                SalaryDisplay = "ETB 14,000 / month (Fellowship Stipend)",
                Deadline = new DateTime(2026, 10, 01),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 92,
                ViewsCount = 810,
                Eligibility = "Pre-final and Final Year Students",
                ExperienceLevel = "Student Fellowship",
                ApplicationEmail = "fellowship@insa.gov.et",
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            },

            new JobPostingViewModel
            {
                Id = 8,
                Title = "AI & Machine Learning Research Assistant",
                Slug = "ai-machine-learning-research-assistant-eaii",
                CompanyName = "Ethiopian Artificial Intelligence Institute",
                CompanyInitials = "EAII",
                CompanyColor = "#059669",
                Industry = "Artificial Intelligence & R&D",
                JobType = "Internship",
                WorkplaceType = "Remote",
                Location = "Remote / Ethiopia",
                CampusLocation = "Remote Access with Hawassa AI Lab",
                ShortDescription = "Contribute to NLP for local Ethiopian languages, computer vision in agriculture, and LLM fine-tuning.",
                Description = "EAII is seeking ambitious student researchers to collaborate on open-source datasets and deep learning models for Amharic, Afaan Oromo, Sidama, and Tigrinya NLP, as well as drone-based computer vision for crop disease detection.",
                Requirements = new List<string>
                {
                    "Strong background in Python, PyTorch / TensorFlow, and linear algebra",
                    "Familiarity with Hugging Face Transformers, tokenization, or OpenCV",
                    "Passion for developing AI applications addressing local African challenges"
                },
                Responsibilities = new List<string>
                {
                    "Preprocess and curate multilingual audio and text datasets",
                    "Fine-tune transformer models for translation, transcription, and text generation",
                    "Evaluate model accuracy and document experimental findings"
                },
                Skills = new List<string> { "Python", "PyTorch", "NLP", "Transformers", "Computer Vision", "Git" },
                SalaryDisplay = "ETB 12,500 / month (Research Grant)",
                Deadline = new DateTime(2026, 9, 20),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 47,
                ViewsCount = 460,
                Eligibility = "Students & Enthusiasts with AI Projects",
                ExperienceLevel = "Student Researcher",
                ApplicationEmail = "research@eaii.gov.et",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },

            new JobPostingViewModel
            {
                Id = 9,
                Title = "Cloud Systems Junior Administrator",
                Slug = "cloud-systems-junior-administrator-sidama-bank",
                CompanyName = "Sidama Bank",
                CompanyInitials = "SB",
                CompanyColor = "#d97706",
                Industry = "Banking & Cloud IT",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Head Office",
                CampusLocation = "Hawassa City Center",
                ShortDescription = "Manage on-premise virtualized servers, cloud backup solutions, and enterprise disaster recovery.",
                Description = "Sidama Bank Head Office in Hawassa is looking for an energetic Junior System Administrator to support our core banking servers, VMware clusters, automated backup solutions, and Active Directory domains.",
                Requirements = new List<string>
                {
                    "B.Sc. in Computer Science, IT, or Computer Engineering",
                    "Hands-on experience with Windows Server, Active Directory, and Linux CLI",
                    "Basic understanding of virtualization (VMware ESXi / Proxmox) and containerization",
                    "Ability to work effectively in on-call rotations when needed"
                },
                Responsibilities = new List<string>
                {
                    "Provision, configure, and monitor physical and virtual server infrastructure",
                    "Administer domain user accounts, permissions, and security group policies",
                    "Verify daily database backups and test disaster recovery protocols"
                },
                Skills = new List<string> { "Windows Server", "VMware", "Active Directory", "Linux", "Backup & Recovery" },
                SalaryDisplay = "ETB 19,000 - 25,000 / month",
                Deadline = new DateTime(2026, 9, 30),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 33,
                ViewsCount = 280,
                Eligibility = "Graduates in CS, IT, or Computer Engineering",
                ExperienceLevel = "Entry Level (0-2 yrs)",
                ApplicationEmail = "hr@sidamabank.et",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            new JobPostingViewModel
            {
                Id = 10,
                Title = "Graphic & UI/UX Designer (Part-Time)",
                Slug = "graphic-uiux-designer-creative-hub-ethiopia",
                CompanyName = "Creative Hub Ethiopia",
                CompanyInitials = "CHE",
                CompanyColor = "#db2777",
                Industry = "Design & Media",
                JobType = "Part-Time",
                WorkplaceType = "Remote",
                Location = "Remote / Hawassa",
                CampusLocation = "Remote Work",
                ShortDescription = "Design visually appealing brand identities, social media creatives, and web app prototypes.",
                Description = "Creative Hub Ethiopia collaborates with brands and campus initiatives to create modern graphics, posters, branding kits, and mobile UI designs. Perfect part-time gig for design-savvy Hawassa University students.",
                Requirements = new List<string>
                {
                    "Proficiency with Figma, Adobe Photoshop, and Illustrator",
                    "Strong visual design sense: typography, color theory, spacing, and micro-interactions",
                    "Portfolio demonstrating previous graphic or web UI work"
                },
                Responsibilities = new List<string>
                {
                    "Create social media banners, event flyers, and presentation decks",
                    "Design user flows and high-fidelity wireframes in Figma",
                    "Collaborate with marketing teams on campaign visuals"
                },
                Skills = new List<string> { "Figma", "Adobe Photoshop", "Illustrator", "UI/UX", "Typography" },
                SalaryDisplay = "ETB 7,000 - 10,000 / month",
                Deadline = new DateTime(2026, 9, 12),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 29,
                ViewsCount = 240,
                Eligibility = "Students with Design Portfolios",
                ExperienceLevel = "Student / Freelance",
                ApplicationEmail = "design@creativehub.et",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new JobPostingViewModel
            {
                Id = 11,
                Title = "Digital Marketing & Events Coordinator",
                Slug = "digital-marketing-events-coordinator-hucems",
                CompanyName = "HUCEMS Student Council",
                CompanyInitials = "HUC",
                CompanyColor = "#8b5cf6",
                Industry = "Campus Life & Marketing",
                JobType = "Part-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Main Campus",
                CampusLocation = "Student Union Building",
                ShortDescription = "Coordinate campus event publicity, manage social media channels, and drive student engagement.",
                Description = "Represent HUCEMS on campus! Help manage university event announcements, write engaging articles for the community tab, organize registration booths at campus festivals, and boost student participation.",
                Requirements = new List<string>
                {
                    "Active Hawassa University student passionate about campus life and event organizing",
                    "Experience with Telegram channel management, TikTok/Instagram content creation",
                    "Energetic, outgoing personality and excellent teamwork skills"
                },
                Responsibilities = new List<string>
                {
                    "Create promotional announcements and campus event teasers",
                    "Assist organizers with QR ticket check-ins and attendee registration at live events",
                    "Gather attendee feedback and prepare event highlight summaries"
                },
                Skills = new List<string> { "Social Media", "Content Creation", "Event Coordination", "Public Speaking" },
                SalaryDisplay = "ETB 4,500 / month + Event Passes",
                Deadline = new DateTime(2026, 8, 25),
                IsClosingSoon = true,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 60,
                ViewsCount = 490,
                Eligibility = "All Hawassa Students",
                ExperienceLevel = "Campus Role",
                ApplicationEmail = "council@hucems.hu.edu.et",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },

            new JobPostingViewModel
            {
                Id = 12,
                Title = "Embedded Systems & IoT Intern",
                Slug = "embedded-systems-iot-intern-hawassa-iot",
                CompanyName = "Hawassa Institute of Technology (IoT)",
                CompanyInitials = "IoT",
                CompanyColor = "#0891b2",
                Industry = "Hardware & Embedded Systems",
                JobType = "Internship",
                WorkplaceType = "On-site",
                Location = "Hawassa IoT Campus",
                CampusLocation = "IoT Hardware & Robotics Laboratory",
                ShortDescription = "Build smart microcontroller projects, sensor telemetry systems, and automated IoT devices.",
                Description = "Join the IoT Research Lab at the Hawassa Institute of Technology. Work with Arduino, ESP32, Raspberry Pi, LoRaWAN modules, and cloud IoT dashboards on real agricultural and industrial automation prototypes.",
                Requirements = new List<string>
                {
                    "Enrolled in Electrical & Computer Engineering, Mechatronics, or Computer Science",
                    "Experience programming in C/C++ or MicroPython for microcontrollers",
                    "Basic circuit debugging, multimeter usage, and soldering skills"
                },
                Responsibilities = new List<string>
                {
                    "Design circuit schematics and assemble sensor prototype breadboards",
                    "Write firmware to read sensor data and transmit packets via MQTT / HTTP",
                    "Test device battery consumption and wireless signal strength in field trials"
                },
                Skills = new List<string> { "C/C++", "ESP32", "Arduino", "IoT Protocols (MQTT)", "Circuit Design" },
                SalaryDisplay = "ETB 7,500 / month (Lab Stipend)",
                Deadline = new DateTime(2026, 10, 15),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 22,
                ViewsCount = 210,
                Eligibility = "Engineering & IoT Students",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "iot-lab@hu.edu.et",
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            },

            new JobPostingViewModel
            {
                Id = 13,
                Title = "Backend Engineer (ASP.NET Core / C#)",
                Slug = "backend-engineer-aspnet-core-dashen-bank",
                CompanyName = "Dashen Bank FinTech Hub",
                CompanyInitials = "DB",
                CompanyColor = "#1e3a8a",
                Industry = "Banking & FinTech",
                JobType = "Full-Time",
                WorkplaceType = "Hybrid",
                Location = "Addis Ababa & Hawassa, Ethiopia",
                CampusLocation = "Regional Tech Center",
                ShortDescription = "Build high-throughput payment APIs, microservices, and secure transaction workflows.",
                Description = "Dashen Bank's Digital Banking division is expanding its developer team. We build high-volume digital wallet systems, merchant payment APIs, and banking integration microservices in C# and ASP.NET Core.",
                Requirements = new List<string>
                {
                    "B.Sc. in Computer Science or Software Engineering",
                    "Strong proficiency in C#, ASP.NET Core Web API, Entity Framework Core",
                    "Understanding of microservices architecture, RabbitMQ/Kafka, and Docker",
                    "Knowledge of database optimization, indexing, and transactional ACID principles"
                },
                Responsibilities = new List<string>
                {
                    "Design and implement secure RESTful and gRPC service endpoints",
                    "Integrate payment switches, third-party billers, and SMS/notification gateways",
                    "Conduct automated regression testing and performance load tests"
                },
                Skills = new List<string> { "ASP.NET Core", "C#", "SQL Server", "Docker", "Microservices", "Redis" },
                SalaryDisplay = "ETB 22,000 - 30,000 / month + Bonus",
                Deadline = new DateTime(2026, 10, 05),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 55,
                ViewsCount = 510,
                Eligibility = "Graduates with Strong C# Skills",
                ExperienceLevel = "Junior to Mid-Level",
                ApplicationEmail = "careers@dashenbanksc.com",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },

            new JobPostingViewModel
            {
                Id = 14,
                Title = "Agricultural Tech & GIS Specialist",
                Slug = "agricultural-tech-gis-specialist-hu-agri",
                CompanyName = "Hawassa College of Agriculture",
                CompanyInitials = "HCA",
                CompanyColor = "#15803d",
                Industry = "Agriculture & GIS",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Agriculture Campus",
                CampusLocation = "College of Agriculture Research Station",
                ShortDescription = "Apply GIS mapping, drone imagery, and soil data analysis for precision farming research.",
                Description = "Hawassa University College of Agriculture is hiring a Junior GIS & Ag-Tech Specialist to support ongoing smart farming and irrigation research projects in the Rift Valley basin.",
                Requirements = new List<string>
                {
                    "Degree in Plant Sciences, Agricultural Engineering, GIS / Remote Sensing, or Environmental Science",
                    "Experience with ArcGIS, QGIS, or Google Earth Engine",
                    "Fieldwork experience in soil sampling and crop telemetry"
                },
                Responsibilities = new List<string>
                {
                    "Process satellite multispectral imagery and drone orthomosaics",
                    "Map crop stress indicators and soil moisture variations",
                    "Assist research professors in preparing technical field reports"
                },
                Skills = new List<string> { "QGIS", "ArcGIS", "Remote Sensing", "Data Analysis", "Field Research" },
                SalaryDisplay = "ETB 15,000 - 19,000 / month",
                Deadline = new DateTime(2026, 9, 18),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 18,
                ViewsCount = 175,
                Eligibility = "Agri, GIS & Environmental Graduates",
                ExperienceLevel = "Fresh Graduate",
                ApplicationEmail = "agri-research@hu.edu.et",
                CreatedAt = DateTime.UtcNow.AddDays(-9)
            },

            new JobPostingViewModel
            {
                Id = 15,
                Title = "Quality Assurance (QA) Software Tester",
                Slug = "quality-assurance-qa-tester-addis-software",
                CompanyName = "Addis Software",
                CompanyInitials = "AS",
                CompanyColor = "#0284c7",
                Industry = "Software & IT",
                JobType = "Internship",
                WorkplaceType = "Remote",
                Location = "Remote, Ethiopia",
                CampusLocation = "Work From Anywhere",
                ShortDescription = "Test web and mobile apps, write automated test cases in Cypress/Selenium, and report bugs.",
                Description = "Addis Software is an export-focused software development house. We are looking for detailed-oriented student QA interns to join our quality engineering team.",
                Requirements = new List<string>
                {
                    "Student in Computer Science or Software Engineering with strong analytical mindset",
                    "Understanding of manual testing methodologies, test cases, and bug lifecycle",
                    "Bonus: familiarity with Cypress, Postman API testing, or Playwright"
                },
                Responsibilities = new List<string>
                {
                    "Execute regression and sanity test suites on web and mobile platforms",
                    "Write clear, reproducible bug tickets on Jira with screenshots and logs",
                    "Validate API responses against OpenAPI contracts using Postman"
                },
                Skills = new List<string> { "QA Testing", "Postman", "Cypress", "Jira", "Manual Testing" },
                SalaryDisplay = "ETB 8,000 / month (Remote Stipend)",
                Deadline = new DateTime(2026, 9, 28),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 37,
                ViewsCount = 310,
                Eligibility = "Students in CS & IT",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "talent@addissoftware.com",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            new JobPostingViewModel
            {
                Id = 16,
                Title = "Financial Analyst Intern",
                Slug = "financial-analyst-intern-awash-bank",
                CompanyName = "Awash Bank",
                CompanyInitials = "AB",
                CompanyColor = "#b91c1c",
                Industry = "Banking & Finance",
                JobType = "Internship",
                WorkplaceType = "On-site",
                Location = "Hawassa Branch, Sidama",
                CampusLocation = "Hawassa Commercial Hub",
                ShortDescription = "Learn credit analysis, loan risk assessment, and financial reporting at Awash Bank.",
                Description = "Awash Bank invites 3rd and 4th-year Accounting, Finance, and Economics students from Hawassa University for a structured summer internship at our Hawassa branches.",
                Requirements = new List<string>
                {
                    "Enrolled in Accounting, Finance, Economics, or Business Administration",
                    "Strong foundational understanding of financial statements and auditing principles",
                    "Proficiency in Microsoft Excel and quantitative analysis"
                },
                Responsibilities = new List<string>
                {
                    "Assist loan officers in customer financial background verifications",
                    "Reconcile daily ledger reports and transaction balances",
                    "Support customer onboarding and digital banking activations"
                },
                Skills = new List<string> { "Financial Analysis", "Excel", "Accounting", "Auditing", "Banking" },
                SalaryDisplay = "ETB 7,500 / month (Paid Internship)",
                Deadline = new DateTime(2026, 10, 10),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 44,
                ViewsCount = 380,
                Eligibility = "Finance & Accounting Students",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "careers@awashbank.com",
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            },

            new JobPostingViewModel
            {
                Id = 17,
                Title = "Health Informatics & Hospital Systems Specialist",
                Slug = "health-informatics-specialist-hu-hospital",
                CompanyName = "Hawassa Comprehensive Specialized Hospital",
                CompanyInitials = "HUCSH",
                CompanyColor = "#0d9488",
                Industry = "Healthcare & IT",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Referral Hospital",
                CampusLocation = "College of Medicine & Health Sciences",
                ShortDescription = "Maintain electronic medical records (EMR), hospital networks, and medical telemetry systems.",
                Description = "Hawassa Comprehensive Specialized Hospital is looking for a Health Informatics / IT Specialist to support our Bahmni/OpenMRS electronic health records system and clinical IT infrastructure.",
                Requirements = new List<string>
                {
                    "Degree in Health Informatics, Computer Science, or Information Systems",
                    "Experience with healthcare software (OpenMRS, Bahmni, or DHIS2)",
                    "High regard for patient data privacy and HIPAA/medical confidentiality standards"
                },
                Responsibilities = new List<string>
                {
                    "Administer EMR server instances, user accounts, and patient registry databases",
                    "Train doctors, nurses, and pharmacists on clinical data entry modules",
                    "Generate epidemiologic and hospital utilization reports for health authorities"
                },
                Skills = new List<string> { "Health Informatics", "OpenMRS", "Database Management", "Clinical IT", "DHIS2" },
                SalaryDisplay = "ETB 16,500 - 21,000 / month",
                Deadline = new DateTime(2026, 10, 20),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 20,
                ViewsCount = 190,
                Eligibility = "Health Informatics & CS Graduates",
                ExperienceLevel = "Fresh Graduate",
                ApplicationEmail = "hospital-hr@hu.edu.et",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },

            new JobPostingViewModel
            {
                Id = 18,
                Title = "Junior DevOps & Cloud Engineer",
                Slug = "junior-devops-cloud-engineer-gebeya",
                CompanyName = "Gebeya Inc.",
                CompanyInitials = "GEB",
                CompanyColor = "#eab308",
                Industry = "Software & Cloud",
                JobType = "Full-Time",
                WorkplaceType = "Remote",
                Location = "Remote, Ethiopia",
                CampusLocation = "Remote Distributed Team",
                ShortDescription = "Automate CI/CD pipelines, manage Kubernetes clusters, and optimize cloud infrastructure on AWS.",
                Description = "Gebeya Inc. connects African tech talent with global software opportunities. We are looking for an ambitious Junior DevOps Engineer to maintain our developer infrastructure and cloud deployments.",
                Requirements = new List<string>
                {
                    "Degree in Computer Science, Software Engineering, or equivalent experience",
                    "Hands-on experience with Docker, GitHub Actions, and Linux shell scripting (Bash)",
                    "Basic knowledge of AWS or Azure cloud primitives (EC2, S3, RDS, IAM)",
                    "Eagerness to learn Terraform and Kubernetes"
                },
                Responsibilities = new List<string>
                {
                    "Build and maintain automated GitHub Actions continuous integration pipelines",
                    "Monitor server health, logging (ELK / Grafana), and resource autoscaling",
                    "Assist engineering squads with containerized development environments"
                },
                Skills = new List<string> { "Docker", "CI/CD", "AWS", "Linux", "GitHub Actions", "Kubernetes" },
                SalaryDisplay = "ETB 25,000 - 35,000 / month",
                Deadline = new DateTime(2026, 9, 22),
                IsClosingSoon = false,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 49,
                ViewsCount = 470,
                Eligibility = "Graduates with DevOps skills",
                ExperienceLevel = "Junior (1-2 yrs)",
                ApplicationEmail = "talent@gebeya.com",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },

            new JobPostingViewModel
            {
                Id = 19,
                Title = "Renewable Energy & Solar Tech Intern",
                Slug = "renewable-energy-solar-tech-intern-eep",
                CompanyName = "Ethiopian Electric Power (EEP)",
                CompanyInitials = "EEP",
                CompanyColor = "#0284c7",
                Industry = "Energy & Engineering",
                JobType = "Internship",
                WorkplaceType = "On-site",
                Location = "Hawassa Regional Office, Sidama",
                CampusLocation = "Regional Power Substation",
                ShortDescription = "Gain practical experience in solar photovoltaic arrays, power grid distribution, and energy monitoring.",
                Description = "Ethiopian Electric Power offers an intensive practical internship for Hawassa University Electrical, Mechanical, and Sustainable Energy engineering students.",
                Requirements = new List<string>
                {
                    "Enrolled in Electrical Power Engineering, Mechanical Engineering, or Energy Studies",
                    "Solid grasp of AC/DC power theory, solar PV systems, and electrical schematics",
                    "Commitment to high electrical safety standards"
                },
                Responsibilities = new List<string>
                {
                    "Inspect solar panel installations and inverter performance data",
                    "Assist senior engineers with power transformer diagnostics and thermal imaging",
                    "Compile daily substation load and power factor metrics"
                },
                Skills = new List<string> { "Solar PV", "Power Systems", "Electrical Schematics", "Safety Standards" },
                SalaryDisplay = "ETB 8,000 / month (Paid Internship)",
                Deadline = new DateTime(2026, 10, 12),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 31,
                ViewsCount = 270,
                Eligibility = "Electrical & Energy Engineering Students",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "internships@eep.com.et",
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            },

            new JobPostingViewModel
            {
                Id = 20,
                Title = "Technical Content Writer & Documentation Lead",
                Slug = "technical-content-writer-africa-tech-review",
                CompanyName = "Africa Tech Review",
                CompanyInitials = "ATR",
                CompanyColor = "#6366f1",
                Industry = "Tech Journalism & Media",
                JobType = "Part-Time",
                WorkplaceType = "Remote",
                Location = "Remote / Ethiopia",
                CampusLocation = "Remote Position",
                ShortDescription = "Write engaging developer tutorials, tech news articles, and university innovation spotlights.",
                Description = "Write about emerging African technology, software development tutorials, campus student hackathons, and AI developments for Africa Tech Review's weekly publication.",
                Requirements = new List<string>
                {
                    "Strong English writing, editing, and storytelling skills",
                    "Interest in software engineering, startup ecosystems, and tech trends",
                    "Ability to explain complex technical concepts in an accessible, engaging manner"
                },
                Responsibilities = new List<string>
                {
                    "Publish 2-3 in-depth tech articles or developer guides per week",
                    "Interview campus founders, student project teams, and tech leaders",
                    "Format content with clean Markdown and code snippets"
                },
                Skills = new List<string> { "Technical Writing", "Markdown", "Content Strategy", "Tech Journalism" },
                SalaryDisplay = "ETB 6,000 - 9,000 / month",
                Deadline = new DateTime(2026, 9, 14),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 25,
                ViewsCount = 210,
                Eligibility = "Open to All Passionate Writers",
                ExperienceLevel = "Student / Freelance",
                ApplicationEmail = "editor@africatechreview.org",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new JobPostingViewModel
            {
                Id = 21,
                Title = "Human Resources & Recruitment Assistant",
                Slug = "hr-recruitment-assistant-bgi-ethiopia",
                CompanyName = "BGI Ethiopia",
                CompanyInitials = "BGI",
                CompanyColor = "#ca8a04",
                Industry = "Manufacturing & FMCG",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Plant, Sidama",
                CampusLocation = "Hawassa Industrial Zone",
                ShortDescription = "Support employee onboarding, staff training coordination, and candidate interview scheduling.",
                Description = "BGI Ethiopia Hawassa Brewery is hiring an energetic HR Assistant to facilitate employee relations, campus recruitment drives, payroll record updates, and company welfare programs.",
                Requirements = new List<string>
                {
                    "Degree in Management, Human Resources, Business Administration, or Psychology",
                    "Strong interpersonal, organization, and conflict-resolution abilities",
                    "Proficiency with MS Office (Word, Excel, PowerPoint)"
                },
                Responsibilities = new List<string>
                {
                    "Coordinate candidate interview schedules and send applicant communications",
                    "Prepare employee onboarding packages and orientation presentations",
                    "Maintain confidential personnel files and attendance logs"
                },
                Skills = new List<string> { "Human Resources", "Recruitment", "Interpersonal Skills", "MS Office" },
                SalaryDisplay = "ETB 15,500 - 19,000 / month + Staff Transport",
                Deadline = new DateTime(2026, 9, 16),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 58,
                ViewsCount = 450,
                Eligibility = "Management & Business Graduates",
                ExperienceLevel = "Fresh Graduate",
                ApplicationEmail = "careers-hawassa@bgiethiopia.com",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new JobPostingViewModel
            {
                Id = 22,
                Title = "Supply Chain & Logistics Trainee",
                Slug = "supply-chain-logistics-trainee-hawassa-industrial-park",
                CompanyName = "Hawassa Industrial Park (HIP)",
                CompanyInitials = "HIP",
                CompanyColor = "#0f766e",
                Industry = "Logistics & Supply Chain",
                JobType = "Full-Time",
                WorkplaceType = "On-site",
                Location = "Hawassa Industrial Park",
                CampusLocation = "HIP Logistics Hub",
                ShortDescription = "Manage inventory warehousing, customs documentation, and supply logistics for export factories.",
                Description = "Join the central operations management team at the Hawassa Industrial Park. Learn international export shipping workflows, raw material inventory auditing, and warehouse fleet management.",
                Requirements = new List<string>
                {
                    "B.Sc. in Supply Chain Management, Logistics, Industrial Engineering, or Business Administration",
                    "Good understanding of warehouse management systems (WMS) and inventory tracking",
                    "Strong verbal and written English communication"
                },
                Responsibilities = new List<string>
                {
                    "Track inbound container shipments and coordinate customs clearance documentation",
                    "Perform weekly physical stock reconciliation in central dry storage warehouses",
                    "Generate shipping manifest reports for park tenant factories"
                },
                Skills = new List<string> { "Supply Chain", "Logistics", "Inventory Management", "Customs Export", "ERP" },
                SalaryDisplay = "ETB 16,000 - 20,000 / month",
                Deadline = new DateTime(2026, 10, 02),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = false,
                IsVerifiedEmployer = true,
                ApplicantCount = 40,
                ViewsCount = 320,
                Eligibility = "Logistics & Engineering Graduates",
                ExperienceLevel = "Entry Level",
                ApplicationEmail = "recruitment@hip.gov.et",
                CreatedAt = DateTime.UtcNow.AddDays(-11)
            },

            new JobPostingViewModel
            {
                Id = 23,
                Title = "Robotics & Automation Research Intern",
                Slug = "robotics-automation-research-intern-hawassa-fablab",
                CompanyName = "Hawassa FabLab & Innovation Center",
                CompanyInitials = "FAB",
                CompanyColor = "#7c3aed",
                Industry = "Robotics & Hardware",
                JobType = "Internship",
                WorkplaceType = "On-site",
                Location = "Hawassa IoT Campus",
                CampusLocation = "FabLab Innovation Workshop",
                ShortDescription = "Work with 3D printers, CNC laser cutters, robotic arms, and autonomous rover prototypes.",
                Description = "The Hawassa FabLab is a rapid-prototyping paradise for student makers! Join our research internship to design robotic actuators, print custom 3D mechanisms, and develop computer vision navigation algorithms for autonomous mobile robots.",
                Requirements = new List<string>
                {
                    "Undergraduate student in Mechanical, Mechatronics, Electrical, or Software Engineering",
                    "Experience with CAD design (SolidWorks, Fusion 360, or FreeCAD)",
                    "Basic coding knowledge in Python or C++ (ROS / Arduino is a plus)"
                },
                Responsibilities = new List<string>
                {
                    "Operate and maintain 3D printers, laser cutters, and electronics workstations",
                    "Assemble robot chassis, motor drivers, and battery management circuits",
                    "Help conduct maker workshops for fellow Hawassa University students"
                },
                Skills = new List<string> { "Robotics", "3D Printing", "CAD / SolidWorks", "ROS", "Microcontrollers" },
                SalaryDisplay = "ETB 7,500 / month (Maker Stipend)",
                Deadline = new DateTime(2026, 10, 25),
                IsClosingSoon = false,
                IsFeatured = false,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 35,
                ViewsCount = 290,
                Eligibility = "Engineering & Tech Students",
                ExperienceLevel = "Student / Internship",
                ApplicationEmail = "fablab@hu.edu.et",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },

            new JobPostingViewModel
            {
                Id = 24,
                Title = "Campus Brand Ambassador & Tech Lead",
                Slug = "campus-brand-ambassador-gdsc-hawassa",
                CompanyName = "Google Developer Student Clubs",
                CompanyInitials = "GDSC",
                CompanyColor = "#ea4335",
                Industry = "Developer Community",
                JobType = "Part-Time",
                WorkplaceType = "Hybrid",
                Location = "Hawassa University",
                CampusLocation = "Across All HU Campuses",
                ShortDescription = "Lead Google tech workshops, organize Solution Challenge hackathons, and mentor student devs.",
                Description = "Represent Google Developer Student Clubs at Hawassa University! Help organize hands-on coding bootcamps on Flutter, Android, Google Cloud, and AI. Lead campus study jams and help student developers submit entries to the global Google Solution Challenge.",
                Requirements = new List<string>
                {
                    "Enthusiastic Hawassa University student active in developer communities and clubs",
                    "Familiarity with modern developer technologies (Web, Mobile, Cloud, or AI)",
                    "Great public speaking and workshop facilitation skills"
                },
                Responsibilities = new List<string>
                {
                    "Host monthly tech talks, hands-on codelabs, and developer study jams",
                    "Collaborate with the HUCEMS events team to publish and promote tech club workshops",
                    "Connect student developers with industry mentors and Google Developer Experts"
                },
                Skills = new List<string> { "Community Leadership", "Public Speaking", "Google Cloud", "Flutter", "Mentorship" },
                SalaryDisplay = "Community Leadership Grant + Google Swag",
                Deadline = new DateTime(2026, 8, 31),
                IsClosingSoon = true,
                IsFeatured = true,
                IsNew = true,
                IsVerifiedEmployer = true,
                ApplicantCount = 70,
                ViewsCount = 630,
                Eligibility = "All Hawassa University Students",
                ExperienceLevel = "Campus Role",
                ApplicationEmail = "gdsc.hawassa@gmail.com",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        public JobsController(ApplicationDbContext db, ILogger<JobsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // =========================================================
        // GET: /Jobs or /Jobs/Index
        // =========================================================
        public async Task<IActionResult> Index(
            string? search,
            string? type,
            string? workplace,
            string? location,
            string? department,
            string? industry,
            string? sort = "newest")
        {
            // Build working job list starting with master catalog
            var allJobs = new List<JobPostingViewModel>(MasterJobsCatalog);

            // Also check if any database job postings exist and merge them
            try
            {
                if (_db.job_postings != null && await _db.job_postings.AnyAsync())
                {
                    var dbJobs = await _db.job_postings
                        .Include(j => j.employer)
                        .Where(j => j.status == "PUBLISHED" || j.status == "ACTIVE")
                        .OrderByDescending(j => j.created_at)
                        .ToListAsync();

                    foreach (var dbJob in dbJobs)
                    {
                        var mappedJob = new JobPostingViewModel
                        {
                            Id = dbJob.id,
                            Title = dbJob.title,
                            Slug = dbJob.slug ?? dbJob.title.ToLower().Replace(" ", "-"),
                            CompanyName = dbJob.employer?.name ?? "Campus Partner",
                            CompanyLogo = dbJob.employer?.logo_url,
                            CompanyInitials = string.IsNullOrWhiteSpace(dbJob.employer?.name) ? "CP" : dbJob.employer.name.Substring(0, Math.Min(3, dbJob.employer.name.Length)).ToUpper(),
                            CompanyColor = "#6f42c1",
                            Industry = dbJob.employer?.industry ?? "Technology",
                            JobType = FormatJobType(dbJob.job_type),
                            WorkplaceType = FormatWorkplaceType(dbJob.workplace_type),
                            Location = dbJob.location ?? "Hawassa, Ethiopia",
                            CampusLocation = "Hawassa University",
                            Description = dbJob.description ?? string.Empty,
                            ShortDescription = dbJob.description != null && dbJob.description.Length > 150 ? dbJob.description.Substring(0, 147) + "..." : (dbJob.description ?? string.Empty),
                            SalaryDisplay = FormatSalary(dbJob.salary_min, dbJob.salary_max, dbJob.salary_currency),
                            Deadline = dbJob.deadline_at,
                            IsClosingSoon = dbJob.deadline_at.HasValue && dbJob.deadline_at.Value <= DateTime.UtcNow.AddDays(14),
                            IsFeatured = false,
                            IsNew = dbJob.created_at >= DateTime.UtcNow.AddDays(-7),
                            IsVerifiedEmployer = dbJob.employer?.verified ?? true,
                            ApplicationUrl = dbJob.application_url,
                            ApplicationEmail = dbJob.application_email,
                            CreatedAt = dbJob.created_at,
                            Skills = new List<string> { "General", "Professional" }
                        };

                        if (!string.IsNullOrWhiteSpace(dbJob.requirements))
                        {
                            mappedJob.Requirements = dbJob.requirements
                                .Split(new[] { '\r', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(r => r.Trim())
                                .Where(r => !string.IsNullOrWhiteSpace(r))
                                .ToList();
                        }

                        if (!string.IsNullOrWhiteSpace(dbJob.responsibilities))
                        {
                            mappedJob.Responsibilities = dbJob.responsibilities
                                .Split(new[] { '\r', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(r => r.Trim())
                                .Where(r => !string.IsNullOrWhiteSpace(r))
                                .ToList();
                        }

                        allJobs.Insert(0, mappedJob);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not fetch DB jobs; continuing with master catalog.");
            }

            // Calculate category counts for UI badges
            var typeCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "All", allJobs.Count },
                { "Internship", allJobs.Count(j => j.JobType.Equals("Internship", StringComparison.OrdinalIgnoreCase)) },
                { "Full-Time", allJobs.Count(j => j.JobType.Equals("Full-Time", StringComparison.OrdinalIgnoreCase)) },
                { "Part-Time", allJobs.Count(j => j.JobType.Equals("Part-Time", StringComparison.OrdinalIgnoreCase)) },
                { "Remote", allJobs.Count(j => j.WorkplaceType.Equals("Remote", StringComparison.OrdinalIgnoreCase) || j.JobType.Equals("Remote", StringComparison.OrdinalIgnoreCase)) }
            };

            // Apply Search and Filters
            var filtered = allJobs.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var query = search.Trim();
                filtered = filtered.Where(j =>
                    j.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    j.CompanyName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    j.Location.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    j.ShortDescription.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    j.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    j.Skills.Any(s => s.Contains(query, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(type) && !type.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                if (type.Equals("remote", StringComparison.OrdinalIgnoreCase))
                {
                    filtered = filtered.Where(j => j.WorkplaceType.Equals("Remote", StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    filtered = filtered.Where(j => j.JobType.Equals(type, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (!string.IsNullOrWhiteSpace(workplace) && !workplace.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(j => j.WorkplaceType.Equals(workplace, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(location) && !location.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(j => j.Location.Contains(location, StringComparison.OrdinalIgnoreCase) ||
                                               j.CampusLocation.Contains(location, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(industry) && !industry.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(j => j.Industry.Contains(industry, StringComparison.OrdinalIgnoreCase));
            }

            // Apply Sorting
            filtered = sort?.ToLower() switch
            {
                "deadline" => filtered.OrderBy(j => j.Deadline ?? DateTime.MaxValue),
                "popular" => filtered.OrderByDescending(j => j.ViewsCount + j.ApplicantCount),
                "title" => filtered.OrderBy(j => j.Title),
                _ => filtered.OrderByDescending(j => j.IsFeatured).ThenByDescending(j => j.CreatedAt)
            };

            var finalJobsList = filtered.ToList();

            var viewModel = new JobFilterViewModel
            {
                Search = search,
                JobType = type ?? "all",
                WorkplaceType = workplace,
                Location = location,
                Industry = industry,
                SortBy = sort ?? "newest",
                TotalJobs = finalJobsList.Count,
                Jobs = finalJobsList,
                JobTypeCounts = typeCounts,
                AvailableLocations = new List<string> { "All Locations", "Hawassa, Ethiopia", "Addis Ababa, Ethiopia", "Hawassa IoT Campus", "Remote" },
                AvailableIndustries = new List<string> { "All Industries", "Software & IT", "Telecommunications", "Banking & Finance", "Education & ICT", "Cybersecurity & Defense", "Artificial Intelligence & R&D", "Agriculture & GIS", "Design & Media" }
            };

            return View(viewModel);
        }

        // =========================================================
        // GET: /Jobs/Details/5
        // =========================================================
        public IActionResult Details(ulong id)
        {
            var job = MasterJobsCatalog.FirstOrDefault(j => j.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            // Recommended / Similar Jobs
            ViewBag.SimilarJobs = MasterJobsCatalog
                .Where(j => j.Id != id && (j.Industry == job.Industry || j.JobType == job.JobType))
                .Take(3)
                .ToList();

            return View(job);
        }

        // =========================================================
        // GET: /Jobs/GetJobJson/5 (For Quick View Modal)
        // =========================================================
        [HttpGet]
        public IActionResult GetJobJson(ulong id)
        {
            var job = MasterJobsCatalog.FirstOrDefault(j => j.Id == id);
            if (job == null)
            {
                return NotFound(new { success = false, message = "Job not found." });
            }

            return Json(new
            {
                success = true,
                id = job.Id,
                title = job.Title,
                companyName = job.CompanyName,
                companyInitials = job.CompanyInitials,
                companyColor = job.CompanyColor,
                industry = job.Industry,
                jobType = job.JobType,
                workplaceType = job.WorkplaceType,
                location = job.Location,
                campusLocation = job.CampusLocation,
                description = job.Description,
                shortDescription = job.ShortDescription,
                salary = job.SalaryDisplay,
                deadline = job.DeadlineString,
                isClosingSoon = job.IsClosingSoon,
                requirements = job.Requirements,
                responsibilities = job.Responsibilities,
                skills = job.Skills,
                eligibility = job.Eligibility,
                experienceLevel = job.ExperienceLevel,
                applicants = job.ApplicantCount,
                views = job.ViewsCount,
                applicationEmail = job.ApplicationEmail
            });
        }

        // =========================================================
        // GET: /Jobs/Apply/5
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Apply(ulong id)
        {
            var job = MasterJobsCatalog.FirstOrDefault(j => j.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(userIdStr, out ulong currentUserId))
            {
                // Check if already applied
                var hasApplied = await _db.job_applications
                    .AnyAsync(a => a.job_posting_id == id && a.applicant_user_id == currentUserId);

                if (hasApplied)
                {
                    TempData["InfoMessage"] = $"You have already submitted an application for '{job.Title}'. You can track your status below.";
                    return RedirectToAction(nameof(MyApplications));
                }
            }

            var model = new JobApplicationViewModel
            {
                JobId = job.Id,
                JobTitle = job.Title,
                CompanyName = job.CompanyName,
                Location = job.Location,
                JobType = job.JobType,
                FullName = User.Identity?.IsAuthenticated == true ? (User.Identity.Name ?? string.Empty) : string.Empty,
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty
            };

            return View(model);
        }

        // =========================================================
        // POST: /Jobs/Apply
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ulong authenticatedUserId = 1;

            if (!string.IsNullOrEmpty(userIdClaim) && ulong.TryParse(userIdClaim, out ulong parsedUid))
            {
                authenticatedUserId = parsedUid;
            }
            else
            {
                var cleanEmail = model.Email.Trim().ToLower();
                var dbUser = await _db.users.FirstOrDefaultAsync(u => u.email.ToLower() == cleanEmail);
                if (dbUser != null) authenticatedUserId = dbUser.id;
            }

            // Check duplicate application
            var existingApp = await _db.job_applications
                .FirstOrDefaultAsync(a => a.job_posting_id == model.JobId && a.applicant_user_id == authenticatedUserId);

            if (existingApp != null)
            {
                TempData["InfoMessage"] = "You have already submitted an application for this position.";
                return RedirectToAction(nameof(MyApplications));
            }

            var appId = $"APP-HU-{DateTime.UtcNow:yyyyMMdd}-{RandomNumberGenerator.GetInt32(1000, 9999)}";

            try
            {
                var application = new job_application
                {
                    job_posting_id = model.JobId,
                    applicant_user_id = authenticatedUserId,
                    application_code = appId,
                    full_name = model.FullName,
                    email = model.Email,
                    phone = model.Phone,
                    student_id = model.StudentId,
                    department = model.Department,
                    year_of_study = model.YearOfStudy,
                    gpa = model.Gpa,
                    portfolio_url = model.PortfolioUrl,
                    cover_letter = model.CoverLetter,
                    status = "SUBMITTED",
                    applied_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                };

                _db.job_applications.Add(application);

                var audit = new audit_log
                {
                    user_id = authenticatedUserId,
                    action = "JOB_APPLICATION",
                    entity_type = "JOB",
                    entity_id = model.JobId,
                    description = $"{appId}|{model.JobTitle}|{model.CompanyName}|{model.Location}|{model.JobType}|{model.FullName}|{model.Department}|Formal Application Submission",
                    ip_address = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    user_agent = Request.Headers["User-Agent"].ToString(),
                    created_at = DateTime.UtcNow
                };

                _db.audit_logs.Add(audit);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist job application to database.");
            }

            TempData["SuccessMessage"] = $"Your application for '{model.JobTitle}' at {model.CompanyName} was successfully submitted! Reference ID: {appId}.";
            TempData["ApplicationId"] = appId;

            return RedirectToAction(nameof(MyApplications));
        }

        // =========================================================
        // POST: /Jobs/QuickApplyJson (AJAX Modal Endpoint)
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> QuickApplyJson([FromBody] JobApplicationViewModel model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "Invalid application payload." });
            }

            // Secure Identity Enforcement: Resolve strictly from authenticated claims
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !ulong.TryParse(userIdClaim, out ulong authenticatedUserId))
            {
                return Unauthorized(new { success = false, message = "You must be signed in to submit an application." });
            }

            var dbUser = await _db.users.FindAsync(authenticatedUserId);
            if (dbUser == null)
            {
                return Unauthorized(new { success = false, message = "User account not found." });
            }

            // Duplicate Application Check
            var existingApp = await _db.job_applications
                .FirstOrDefaultAsync(a => a.job_posting_id == model.JobId && a.applicant_user_id == authenticatedUserId);

            if (existingApp != null)
            {
                return Json(new { success = false, message = "You have already applied for this position." });
            }

            // Enforce authenticated student credentials (preventing identity spoofing)
            var applicantFullName = $"{dbUser.first_name} {dbUser.last_name}".Trim();
            if (string.IsNullOrWhiteSpace(applicantFullName)) applicantFullName = model.FullName;

            var applicantEmail = dbUser.email;
            var appId = $"APP-HU-{DateTime.UtcNow:yyyyMMdd}-{RandomNumberGenerator.GetInt32(1000, 9999)}";

            try
            {
                var application = new job_application
                {
                    job_posting_id = model.JobId,
                    applicant_user_id = authenticatedUserId,
                    application_code = appId,
                    full_name = applicantFullName,
                    email = applicantEmail,
                    phone = model.Phone,
                    student_id = model.StudentId,
                    department = model.Department,
                    year_of_study = model.YearOfStudy,
                    gpa = model.Gpa,
                    cover_letter = model.CoverLetter ?? "Quick AJAX modal submission",
                    status = "SUBMITTED",
                    applied_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                };

                _db.job_applications.Add(application);

                var audit = new audit_log
                {
                    user_id = authenticatedUserId,
                    action = "JOB_APPLICATION",
                    entity_type = "JOB",
                    entity_id = model.JobId,
                    description = $"{appId}|{model.JobTitle}|{model.CompanyName}|{model.Location}|{model.JobType}|{applicantFullName}|{applicantEmail}|{model.Department}|Quick AJAX Submission",
                    ip_address = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    user_agent = Request.Headers["User-Agent"].ToString(),
                    created_at = DateTime.UtcNow
                };

                _db.audit_logs.Add(audit);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Job application {AppId} successfully recorded for User ID {UserId} ({Email}) on Job {JobId}",
                    appId, authenticatedUserId, applicantEmail, model.JobId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist quick application to database.");
                return StatusCode(500, new { success = false, message = "A database error occurred while saving your application. Please try again." });
            }

            return Json(new
            {
                success = true,
                applicationId = appId,
                message = $"Congratulations {applicantFullName}! Your application for '{model.JobTitle}' at {model.CompanyName} has been submitted successfully."
            });
        }

        // =========================================================
        // GET: /Jobs/MyApplications
        // =========================================================
        public async Task<IActionResult> MyApplications()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value?.Trim().ToLower();
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ulong? uidVal = ulong.TryParse(userIdStr, out ulong uid) ? uid : null;

            var userApplications = new List<StudentApplicationItemViewModel>();

            if (uidVal.HasValue || !string.IsNullOrEmpty(userEmail))
            {
                try
                {
                    // 1. Query relational job_applications table
                    var dbApps = await _db.job_applications
                        .Include(a => a.job_posting)
                        .ThenInclude(j => j.employer)
                        .Where(a => (uidVal.HasValue && a.applicant_user_id == uidVal.Value) || (!string.IsNullOrEmpty(userEmail) && a.email.ToLower() == userEmail))
                        .OrderByDescending(a => a.applied_at)
                        .ToListAsync();

                    foreach (var a in dbApps)
                    {
                        userApplications.Add(new StudentApplicationItemViewModel
                        {
                            ApplicationId = a.application_code,
                            JobId = a.job_posting_id,
                            JobTitle = a.job_posting?.title ?? "University Opportunity",
                            CompanyName = a.job_posting?.employer?.name ?? "Campus Partner",
                            Location = a.job_posting?.location ?? "Hawassa, Ethiopia",
                            JobType = a.job_posting?.job_type ?? "Full-Time",
                            AppliedAt = a.applied_at,
                            Status = a.status switch
                            {
                                "SHORTLISTED" => "Shortlisted",
                                "INTERVIEW_SCHEDULED" => "Interview Scheduled",
                                "UNDER_REVIEW" => "Under Review",
                                "ACCEPTED" => "Accepted",
                                "REJECTED" => "Not Selected",
                                _ => "Application Submitted"
                            },
                            StatusBadgeClass = a.status switch
                            {
                                "SHORTLISTED" => "bg-success-subtle text-success-emphasis",
                                "INTERVIEW_SCHEDULED" => "bg-primary-subtle text-primary-emphasis",
                                "UNDER_REVIEW" => "bg-warning-subtle text-warning-emphasis",
                                "ACCEPTED" => "bg-success text-white",
                                "REJECTED" => "bg-danger-subtle text-danger",
                                _ => "bg-info-subtle text-info-emphasis"
                            },
                            Notes = a.reviewer_notes ?? "Application received and queued for department review."
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not load applications from job_applications table.");
                }
            }

            if (uidVal.HasValue || !string.IsNullOrEmpty(userEmail))
            {
                try
                {
                    var query = _db.audit_logs
                        .Where(a => a.action == "JOB_APPLICATION")
                        .AsQueryable();

                    if (uidVal.HasValue)
                    {
                        query = query.Where(a => a.user_id == uidVal.Value || (userEmail != null && a.description != null && a.description.ToLower().Contains(userEmail)));
                    }
                    else if (!string.IsNullOrEmpty(userEmail))
                    {
                        query = query.Where(a => a.description != null && a.description.ToLower().Contains(userEmail));
                    }

                    var logs = await query.OrderByDescending(a => a.created_at).ToListAsync();

                    foreach (var log in logs)
                    {
                        var parts = (log.description ?? "").Split('|');
                        var appId = parts.Length > 0 ? parts[0] : $"APP-HU-{log.id}";
                        var jTitle = parts.Length > 1 ? parts[1] : "Campus Internship";
                        var cName = parts.Length > 2 ? parts[2] : "Partner Organization";
                        var loc = parts.Length > 3 ? parts[3] : "Hawassa, Ethiopia";
                        var jType = parts.Length > 4 ? parts[4] : "Internship";
                        var applicantName = parts.Length > 5 ? parts[5] : "Student";

                        userApplications.Add(new StudentApplicationItemViewModel
                        {
                            ApplicationId = appId,
                            JobId = log.entity_id ?? 1,
                            JobTitle = jTitle,
                            CompanyName = cName,
                            Location = loc,
                            JobType = jType,
                            ApplicantEmail = userEmail ?? "student@hawassa.edu.et",
                            UserId = userIdStr,
                            AppliedAt = log.created_at,
                            Status = "Under Review",
                            StatusBadgeClass = "bg-primary-subtle text-primary-emphasis",
                            Notes = $"Application verified and recorded in university telemetry registry for {applicantName}."
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load student applications from database.");
                }
            }

            // Fallback sample for display if user has no prior applications
            if (!userApplications.Any() && User.Identity?.IsAuthenticated == true)
            {
                userApplications.Add(new StudentApplicationItemViewModel
                {
                    ApplicationId = "APP-HU-2026-0811",
                    JobId = 1,
                    JobTitle = "Software Developer Intern",
                    CompanyName = "ABC Technology",
                    Location = "Hawassa, Ethiopia",
                    JobType = "Internship",
                    AppliedAt = DateTime.UtcNow.AddDays(-2),
                    Status = "Shortlisted",
                    StatusBadgeClass = "bg-success-subtle text-success-emphasis",
                    Notes = "Resume verified by campus placement coordinator. Assessment pending."
                });
            }

            var vm = new MyApplicationsViewModel
            {
                Applications = userApplications
            };

            return View(vm);
        }

        // =========================================================
        // Helper formatting functions
        // =========================================================
        private static string FormatJobType(string? dbType)
        {
            if (string.IsNullOrWhiteSpace(dbType)) return "Internship";
            return dbType.ToUpper() switch
            {
                "FULL_TIME" => "Full-Time",
                "PART_TIME" => "Part-Time",
                "INTERNSHIP" => "Internship",
                "CONTRACT" => "Contract",
                "VOLUNTEER" => "Volunteer",
                "TEMPORARY" => "Temporary",
                _ => dbType
            };
        }

        private static string FormatWorkplaceType(string? dbType)
        {
            if (string.IsNullOrWhiteSpace(dbType)) return "On-site";
            return dbType.ToUpper() switch
            {
                "ON_SITE" => "On-site",
                "REMOTE" => "Remote",
                "HYBRID" => "Hybrid",
                _ => dbType
            };
        }

        private static string FormatSalary(decimal? min, decimal? max, string? currency)
        {
            currency ??= "ETB";
            if (min.HasValue && max.HasValue)
            {
                return $"{currency} {min:N0} - {max:N0} / mo";
            }
            if (min.HasValue)
            {
                return $"{currency} {min:N0}+ / mo";
            }
            return "Competitive";
        }
    }
}
