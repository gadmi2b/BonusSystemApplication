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
			// Reversed
			CreateMap<Conclusion, ConclusionDTO>().ReverseMap();
			CreateMap<Signatures, SignaturesDTO>().ReverseMap();

			CreateMap<DefinitionDTO, DefinitionVM>().ReverseMap();
			CreateMap<ConclusionDTO, ConclusionVM>().ReverseMap();
			CreateMap<SignaturesDTO, SignaturesVM>().ReverseMap();
			CreateMap<ObjectiveResultDTO, ObjectiveResultVM>().ReverseMap();
			CreateMap<ObjectiveDTO, ObjectiveVM>().ReverseMap();
			CreateMap<ResultDTO, ResultVM>().ReverseMap();

			// Business Logic Layer -> Presentation Layer
			CreateMap<TableRowDTO, TableRowVM> ();
			CreateMap<SelectListsDTO, SelectListsVM> ();
			CreateMap<FormIndexDTO, FormIndexViewModel>()
				.ForMember(dest => dest.SelectLists, opt => opt.MapFrom(src => src.SelectLists))
				.ForMember(dest => dest.TableRows, opt => opt.MapFrom(src => src.TableRows));

			CreateMap<FormDTO, FormEditViewModel>()
				.ForMember(dest => dest.PeriodSelectList, opt => opt.Ignore())
				.ForMember(dest => dest.EmployeeSelectList, opt => opt.Ignore())
				.ForMember(dest => dest.WorkprojectSelectList, opt => opt.Ignore());

			// Business Logic Layer -> Data Access Layer
			CreateMap<DefinitionDTO, Definition>()
				.ForMember(dest => dest.Form, opt => opt.Ignore())
				.ForMember(dest => dest.FormId, opt => opt.Ignore())
				.ForMember(dest => dest.Manager, opt => opt.Ignore())
				.ForMember(dest => dest.Approver, opt => opt.Ignore())
				.ForMember(dest => dest.Employee, opt => opt.Ignore())
				.ForMember(dest => dest.Workproject, opt => opt.Ignore());

			CreateMap<ObjectiveResultDTO, ObjectiveResult>()
				.ForMember(dest => dest.Form, opt => opt.Ignore())
				.ForMember(dest => dest.FormId, opt => opt.Ignore());
			CreateMap<ObjectiveDTO, Objective>();
			CreateMap<ResultDTO, Result>();

			// Data Access Layer -> Business Logic Layer
			CreateMap<Form, FormDTO>()
				.ForPath(dest => dest.Definition, opt => opt.MapFrom(src => src.Definition))
				.ForPath(dest => dest.Definition.Pid, opt => opt.MapFrom(src => src.Definition.Employee.Pid))
				.ForPath(dest => dest.Definition.TeamName, opt => opt.MapFrom(src => src.Definition.Employee.Team.Name))
				.ForPath(dest => dest.Definition.PositionName, opt => opt.MapFrom(src => src.Definition.Employee.Position.NameEng))
				.ForPath(dest => dest.Definition.WorkprojectDescription, opt => opt.MapFrom(src => src.Definition.Workproject.Description));

			CreateMap<Definition, DefinitionDTO>()
				.ForMember(dest => dest.TeamName, opt => opt.Ignore())
				.ForMember(dest => dest.PositionName, opt => opt.Ignore())
				.ForMember(dest => dest.Pid, opt => opt.Ignore());

			CreateMap<ObjectiveResult, ObjectiveResultDTO>();
            CreateMap<Objective, ObjectiveDTO>();
            CreateMap<Result, ResultDTO>();
        }
	}
}
