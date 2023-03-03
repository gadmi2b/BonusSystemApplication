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

			CreateMap<DefinitionDTO, DefinitionVM>().ReverseMap();
			CreateMap<ConclusionDTO, ConclusionVM>().ReverseMap();
			CreateMap<SignaturesDTO, SignaturesVM>().ReverseMap();
			CreateMap<ObjectiveDTO, ObjectiveVM>().ReverseMap();
			CreateMap<ResultDTO, ResultVM>().ReverseMap();
			CreateMap<ObjectiveResultDTO, ObjectiveResultVM>().ReverseMap();

			//CreateMap<List<ObjectiveResultDTO>, List<ObjectiveResultVM>>().ReverseMap();

			// Data Access Layer -> Business Logic Layer
			CreateMap<Form, FormDTO>();
			CreateMap<Definition, DefinitionDTO>();
			CreateMap<Conclusion, ConclusionDTO>();
			CreateMap<Signatures, SignaturesDTO>();
			CreateMap<ObjectiveResult, ObjectiveResultDTO>();
			CreateMap<Objective, ObjectiveDTO>();
			CreateMap<Result, ResultDTO>();
		}
	}
}
