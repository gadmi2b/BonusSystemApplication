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
            //var mapper = config.CreateMapper();
        }


        //[TestCase(1, ExpectedResult = RowStatusEnum.Modified)]
        //[TestCase(2, ExpectedResult = RowStatusEnum.Removed)]
        //[TestCase(3, ExpectedResult = RowStatusEnum.Added)]
        //public RowStatusEnum AutoMapper_ConvertFromByte_IsValid(
        //    byte rowStatusEnum)
        //{
        //    var config = new MapperConfiguration(cfg => cfg.AddProfile<MyProfile>());
        //    var mapper = config.CreateMapper();
        //    return mapper.Map<byte, RowStatusEnum>(rowStatusEnum);
        //}

        //[TestCase(RowStatusEnum.Modified, ExpectedResult = 1)]
        //[TestCase(RowStatusEnum.Removed, ExpectedResult = 2)]
        //[TestCase(RowStatusEnum.Added, ExpectedResult = 3)]
        //public byte AutoMapper_ConvertFromEnum_IsValid(
        //    RowStatusEnum rowStatusEnum)
        //{
        //    var config = new MapperConfiguration(cfg => cfg.AddProfile<MyProfile>());
        //    var mapper = config.CreateMapper();
        //    return mapper.Map<RowStatusEnum, byte>(rowStatusEnum);

        //}
    }
}
