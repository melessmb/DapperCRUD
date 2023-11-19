using AutoMapper;
using LearnDapper.Dtos;
using LearnDapper.Entities;

namespace LearnDapper.Mapping;

public class TodoProfile: Profile
{
    public TodoProfile()
    {
        CreateMap<Todo, TodoDto>();
        CreateMap<TodoDto, Todo>();
    }
}