using HawassaUnifiedCampusEventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    public class CommunityController : Controller
    {
        private static readonly List<CommunityUser> Users = new()
        {
            new CommunityUser
            {
                Id = 1,
                FullName = "Abebe Kebede",
                Username = "abebe",
                Department = "Computer Science",
                Bio = "Student and technology enthusiast.",
                Followers = 120,
                Following = 80
            },

            new CommunityUser
            {
                Id = 2,
                FullName = "Sara Ahmed",
                Username = "sara",
                Department = "Information Technology",
                Bio = "Interested in software development.",
                Followers = 95,
                Following = 110
            },

            new CommunityUser
            {
                Id = 3,
                FullName = "Dawit Tesfaye",
                Username = "dawit",
                Department = "Cyber Security",
                Bio = "Cyber security student.",
                Followers = 150,
                Following = 75
            }
        };

        // IMPORTANT:
        // The list is called CommunityPosts,
        // not Posts, because Posts() is also a method.
        private static readonly List<CommunityPost> CommunityPosts = new()
        {
            new CommunityPost
            {
                Id = 1,
                AuthorName = "Abebe Kebede",
                Content = "Looking forward to the upcoming technology workshop!",
                CreatedAt = DateTime.Now.AddHours(-2),
                Likes = 15,
                Comments = 4
            },

            new CommunityPost
            {
                Id = 2,
                AuthorName = "Sara Ahmed",
                Content = "Our student community meeting was great today.",
                CreatedAt = DateTime.Now.AddHours(-5),
                Likes = 23,
                Comments = 7
            }
        };


        // =====================================================
        // COMMUNITY HOME
        // =====================================================

        public IActionResult Index()
        {
            ViewBag.Users = Users;
            ViewBag.Posts = CommunityPosts;

            return View();
        }


        // =====================================================
        // PROFILE
        // =====================================================

        public IActionResult Profile(int id = 1)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // =====================================================
        // FIND PEOPLE
        // =====================================================

        public IActionResult FindPeople()
        {
            return View(Users);
        }


        // =====================================================
        // FRIENDS
        // =====================================================

        public IActionResult Friends()
        {
            var friends = Users
                .Where(x => x.IsFollowing)
                .ToList();

            return View(friends);
        }


        // =====================================================
        // POSTS
        // =====================================================

        public IActionResult Posts()
        {
            return View(CommunityPosts);
        }


        // =====================================================
        // CREATE POST - GET
        // =====================================================

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }


        // =====================================================
        // CREATE POST - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(CommunityPost post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            post.Id = CommunityPosts.Count == 0
                ? 1
                : CommunityPosts.Max(x => x.Id) + 1;

            post.AuthorName = "Current User";

            post.CreatedAt = DateTime.Now;

            CommunityPosts.Insert(0, post);

            return RedirectToAction(nameof(Posts));
        }


        // =====================================================
        // FOLLOW
        // =====================================================

        [HttpPost]
        public IActionResult Follow(int id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);

            if (user != null && !user.IsFollowing)
            {
                user.IsFollowing = true;
                user.Followers++;
            }

            return RedirectToAction(nameof(FindPeople));
        }


        // =====================================================
        // UNFOLLOW
        // =====================================================

        [HttpPost]
        public IActionResult Unfollow(int id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);

            if (user != null && user.IsFollowing)
            {
                user.IsFollowing = false;

                if (user.Followers > 0)
                {
                    user.Followers--;
                }
            }

            return RedirectToAction(nameof(FindPeople));
        }


        // =====================================================
        // SEARCH PEOPLE
        // =====================================================

        [HttpGet]
        public IActionResult Search(string? query)
        {
            IEnumerable<CommunityUser> results = Users;

            if (!string.IsNullOrWhiteSpace(query))
            {
                results = Users.Where(x =>
                    x.FullName.Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    (x.Username ?? "").Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    (x.Department ?? "").Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase)
                );
            }

            ViewBag.Query = query;

            return View(results.ToList());
        }
    }
}