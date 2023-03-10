using AutoMapper;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.Models.Forms.Edit;
using BonusSystemApplication.Models.Forms.Index;

namespace BonusSystemApplication.Mapper
{
	public class AppMappingProfile : Profile
	{
		public AppMappingProfile()
		{
			// Business Logic Layer -> Presentation Layer
			CreateMap<TableRowDTO, TableRowVM> ();
			CreateMap<SelectListsDTO, SelectListsVM> ();
			CreateMap<FormIndexDTO, FormIndexViewModel>()
				.ForMember(dest => dest.SelectLists, opt => opt.MapFrom(src => src.SelectLists))
				.ForMember(dest => dest.TableRows, opt => opt.MapFrom(src => src.TableRows));

			CreateMap<FormDTO, FormEditViewModel>();
			CreateMap<DefinitionDTO, DefinitionVM>().ReverseMap();
			CreateMap<ConclusionDTO, ConclusionVM>().ReverseMap();
			CreateMap<SignaturesDTO, SignaturesVM>().ReverseMap();
			CreateMap<ObjectiveResultDTO, ObjectiveResultVM>().ReverseMap();
			CreateMap<ObjectiveDTO, ObjectiveVM>().ReverseMap();
			CreateMap<ResultDTO, ResultVM>().ReverseMap();

			// Data Access Layer -> Business Logic Layer
			CreateMap<Form, FormDTO>()
				.ForPath(dest => dest.Definition, opt => opt.MapFrom(src => src.Definition))
				.ForPath(dest => dest.Definition.Pid, opt => opt.MapFrom(src => src.Definition.Employee.Pid))
				.ForPath(dest => dest.Definition.TeamName, opt => opt.MapFrom(src => src.Definition.Employee.Team.Name))
				.ForPath(dest => dest.Definition.PositionName, opt => opt.MapFrom(src => src.Definition.Employee.Position.NameEng))
				.ForPath(dest => dest.Definition.WorkprojectDescription, opt => opt.MapFrom(src => src.Definition.Workproject.Description));

			CreateMap<Definition, DefinitionDTO>();
			CreateMap<Conclusion, ConclusionDTO>().ReverseMap();
			CreateMap<Signatures, SignaturesDTO>();
			CreateMap<ObjectiveResult, ObjectiveResultDTO>();
			CreateMap<Objective, ObjectiveDTO>();
			CreateMap<Result, ResultDTO>();
		}
	}
}
