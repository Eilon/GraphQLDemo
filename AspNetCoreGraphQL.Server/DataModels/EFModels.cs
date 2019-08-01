using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreGraphQL.Server.DataModels
{
    public class Movie
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Language { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public List<CastMember> Cast { get; set; }

    }

    public enum Genre
    {
        Action,
        Musical,
        SciFiFantasy,
        Horror,
    }

    public class CastMember
    {
        public int ID { get; set; }
        [Required]
        public string Character { get; set; }
        public int OrderOfAppearance { get; set; }
        public Actor Actor { get; set; }
    }

    public class Actor
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DOB { get; set; }
    }
}
