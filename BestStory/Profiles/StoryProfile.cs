using AutoMapper;
using BestStory.Dtos;
using BestStory.Models;

namespace BestStory.Profiles
{
    public class StoryProfile : Profile
    {
        public StoryProfile()
        {
            CreateMap<Story, StoryDto>()
                .ForMember(dest => dest.Uri, src => src.MapFrom(story => story.Url))
                .ForMember(dest => dest.PostedBy, src => src.MapFrom(story => story.By));
        }
    }
}
