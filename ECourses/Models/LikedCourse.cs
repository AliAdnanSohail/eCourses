using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECourses.Models
{
    public class LikedCourse
    {
      
        public int Id;
        public string Title;
        public int Likes;
        public int Views;
        public string Image;
        public string First_Name;
        public string Address;
        public DateTime? Created_At;
        public bool isLiked;

        public LikedCourse(int id,string title,int like,int views,string first,string image,string add,DateTime? create,bool isliked)
        {
            Id = id;
            Title = title;
            Likes = like;
            Views = views;
            First_Name = first;
            Address = add;
            Created_At = create;
            isLiked = isliked;
            Image = image;

        }

        public LikedCourse(int id, string title, int like, int views, string first, string image, string add, bool isliked)
        {
            Id = id;
            Title = title;
            Likes = like;
            Views = views;
            First_Name = first;
            Address = add;
            isLiked = isliked;
            Image = image;

        }
    }
}