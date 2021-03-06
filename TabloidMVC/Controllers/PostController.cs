﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private int userId;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository,ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        /// <summary>
        /// Add CreateView for a NewPost
        /// </summary>
        /// <returns></returns>
        ////public IActionResult CreateNewPosts()
        //{
           //// int userId = GetCurrentUserProfileId();
            //var posts = _postRepository.GetAllUserPosts(userId);
            ////return View(posts);
        //}

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetUserPostById(id, userId);
            
           
            var categoryOptions = _categoryRepository.GetAll();

            if (post == null)
            {
                return NotFound();
            }

            var vm = new PostCreateViewModel()
            {



                Post = post,
                CategoryOptions = categoryOptions
            };

            return View(vm);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                vm.Post.IsApproved = true;
                _postRepository.UpdatePost(vm.Post);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
    

                return View(vm);
            }
        }


        // GET: Post/Delete/5
        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetPublishedPostById(id);
        
                

            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post, Comment comment)
        {
            try
            {
              List<Comment> comments = _commentRepository.GetAllPostComments(id);
                
                foreach(var item in comments)
                {
                    _commentRepository.Delete(item.Id);
                }
                
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(post);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
