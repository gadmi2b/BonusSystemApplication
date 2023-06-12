using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.Mapper;

namespace BonusSystemApplication.Test.AutoMapper
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
