using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TrustApplication.Domain.Services;
using TrustApplication.Models;
using TrustApplication.Models.Infrastructure;

namespace TrustApplication.Tests
{
    [TestClass]
    public class FileStorageServiceTests
    {
        private FileStorageService _service;
        private IFileStorageRepository _repository;

        private void Populate(FileStorageContext context)
        {
            var sampleJPG = new FileEntity { 
                Filename = "grass.jpg", 
                StoredDateTime = DateTime.UtcNow.AddDays(-1),
                Content = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/4QAiRXhpZgAATU0AKgAAAAgAAQESAAMAAAABAAEAAAAAAAD/7AARRHVja3kAAQAEAAAAMgAA/9sAQwACAQECAQECAgICAgICAgMFAwMDAwMGBAQDBQcGBwcHBgcHCAkLCQgICggHBwoNCgoLDAwMDAcJDg8NDA4LDAwM/9sAQwECAgIDAwMGAwMGDAgHCAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgAKQBhAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/E208V3f9lG3a6mW2kO+W3jlZYpCv3SQOMis2J1u9Ztf7jzKo465NRzyRxWq7t/bIra8J+Cft5sriRiz5DRIpxu54yfrxXmSlCC12M15HpXwq+H998UPHuk6DZxSSXepXSwfu1LbIzhWPA7Akk9sV+o37NH7NLfs9f2ov9pS3lneQwxQw3DYEe0uxAHof6147/wTD/Yr09fh/a/EjWte1KHVr4m20eygZEWOIdG3H+MkfTBNez/E746T2GrXOn6Vb6feadZkQ3V0X8oxv0+dM5ZvccV+N8W5k8RW9jR2R72XwsuaR1l4Lh55PKlBt2cGCcSbg394E56jtVa48UXmjXv2l7iPCwIqqDu6Nknb3IHOK4vWfjDKml2txZxw3GIy89vHMrXAIB/1eDjb6jGa5rU/2g7rVNFkW10u8aOST5JEKloVHLge+AeM9q+Sp0pT3PY9oaf7RHjnUtUvmuLfzrg2UY2KAWWQEcjIHA65I6e3WvBfjf4mHiS8sV3NJcQ24iazkhaKMhfm++eMds13U/xYuPEGv3k3kxxWsyqZo4pAWYDvno2ehVevTFYfxH+Mv9mnTBqK6PfSQsGEMRQTCIHJBHY4HevdwF6TVkcmIqHifx81GO++I+o+Ikht7VNSvRFLFbyER7flVAmP4sjO4dDz9c7/AIJ+f8FQfEX/AAT48b6xq3hnQbXVL/XLsvqUs2oSrb3ao5MYVACFZVaQdMnco7cet6n8atO1jwYLPWdLi1K3vHYfZ0iXdbx9CQx4LD6gZNfKvxr0Dwnda811dx32n3k0wSSKxjUxXrFsLcFThU4O0onPUgd6/SeHsYpXi0fNYtu5/Rd/wSZ/4OA/BP7fVzp/hnXfsvg/x/MWEWiT3wmFyEUuWtpmRRMAiszKuSqqTxgmv0mS7V0VmZVZhkAnnqB/UfnX8cf7A37Xl9+wp+0FpvjLw/aNrlnblYr5L1RHNIm4bXt8f6uVYwowcZJIPFfrH+0P/wAHRFp4T0SCH4f+HV1ya6syWuJp/IW3nBCxrIjMC3yksxXHI696+mqYnl91nDTxDXxH7c/al9V/76X/ABor+eX/AIit/iH/ANCXp3/gbJ/8XRWH1tG31mJ+W+ofsn6y0W23urfBAYb0PU8L17E4rsPDn7L3iIaZZ2sMbtdra4ZIbZmbhueQOMdfYc195aNZ2eoaHcxaNFbXEx2xxsFVbiNU+bfk8HbjPTpWhaeDPJvdOvr2a2t7iV2Vre3kDR5253tg8SY5HYcV+RYnxArqDjKKOHD+2ZX+HHxg0zwJ8BPC/hyKx1CG60ZIklt3gkQOwTMiNgZG4e2RnNYnwx8RwWfxK/ttrS5bdO7I10ksttg8kbSuMjtk+lfQfg/4eWv9lLtka3uLbzIxG1xv82XGFlzj7vOa37I29ppaWcyyTLGpDkt8pyPvEbeRwTX53V4mXPJ2+I+lowrKKPn7xJ8UdPMslhbXUOmSCcukbWTA7zyOeCB0OPT2ri/E1z4b8R3F1fQTW+j327yrq08p/JVwcb+MY+bGR1NfVF58ONN1/UvtIsbVlCx2ayRKNomEbAnJ9lX9KydP8BNqV2YFs7O4sY9tveuYE3CZM7jnH8XB/CsafEUEdPNW6nzfeeCbePTrXatnFJffvY4ZkeOFIj/Dn7y+vYjOea8/+Ifw5ttP066XR20yO8vHVpLWwuJpLSADqX3DOSM98GvtjV/AFmdR0+6vIW+xaX8iSOgCuvU7vXA7ZqTV/CWl380hNra2bMQxzbhUkQcfePH9K6MPxYoS1RnUhVep+YPiTw1rfh57q5tdMm1XczxOscnltGzSDBUe/Uf/AFq4b4i/BnX/ABbp1vqH2P8A0lId/k7svKmeTnGGx045/Gv131X4I+Hda8P273Gj2V5HqkAEkc0OxUTO0PkfeX3zg/hXLyfBjS/+Eemup9A0+OzWT95ELaMedEvCqgB3JjBPGOx5r6fC+I8aGigcc8HUm7s/HiX4Q+KLexZl027Hl4LKkJZUB6bsDAyemcUtv8KfGCW/mt4d1WRc/d+xNhz9cdz/AJNfs7eeEvDfgOaP7LocMA1GPzptsG6O3lA6McYCjjGe9cZ498KWVzrWibfsotY2czbZjDKxYhFdWIwMMQAxOAxHtXsUfFFT19mrmNTLZn5G/wDCrfEn/QI1P/wHaiv1g/4WZpP/AFFv/AI//E0Vv/xE6X/Pkx/s+Rf8I+GY/hP4UludQ8zUL/VruNLOx8o+fBKyFFt+BnJY/N7Ek16H4G8B2t1qlzql9p8k15DHHHCVj/dhjCxO0Y54yM+1cb8X/wDksXhT/rtd/wDoiGvdvCH/ACEtW/6+ov8A0Na/HcyrScVI7sLRjyo42/8AFFjpGp2sc1jDHdzMBG4BYH90xYccbgy4PoeKl1HWLQeDtP1KxC30xELSRI4/eDYzKpPbcP5153bf8jXZf9hi+/8AQ5ql/wCaLx/9eEH/AKQ15HJdanqR0PS7Px4JfBYkcWenrJBE6PkOkUxB3NkcNtO0H34NLovjWGXwjfNL50m4NAWiiK+ZMuRvAxk8jHHofSvHvFP/ACS7xN/1/ad/6Pjr1vWf+RIt/wDr6T/0Y1Kph4qKkHMGheNbjxEszwPZXFraylgypvXA6/iDV6/1mG78K3l5ZBodR1BTCpnUSIdylmOwc7FIG7A4Fcj+zx/yTC9/653f/ow12fgr/kEab/u3P/oD1NakovQa1MLVpLjTdGsbpdVtoy1ilo9vFiOFsNnd83Uew7CtC78VNotjbrdW58m3dFluEG1EyRxkjv6GvM7j7um/9fUv/pStek3v/ICuv+vuz/8ARVHLzbj5iHTtXg8YalrWiTPBcX0TRSW9ywHlyI434XsxXg4GffFP8T3un+HPskK6bNJcalcmKKGDbIsg8zcGLdlWRec8DnpivNvhB/yIGkf9fTf+hPXo9v8A8gfTf+utx/6OlrTl9nPQV7o5b/hHl/59bz/vlf8ACivPKK7udisf/9k="
            };

            context.Add(sampleJPG);

            context.SaveChanges();
        }
        private IFileStorageRepository GetInMemoryRepository()
        {
            var options = new DbContextOptionsBuilder<FileStorageContext>()
                             .UseInMemoryDatabase(databaseName: "MockDB")
                             .Options;

            var initContext = new FileStorageContext(options);

            initContext.Database.EnsureDeleted();

            Populate(initContext);

            var testContext = new FileStorageContext(options);

            var repository = new FileStorageRepository(testContext);

            return repository;
        }

        [TestInitialize]
        public void Setup()
        {
            _repository = GetInMemoryRepository();
            _service = new FileStorageService(_repository);
        }

        [TestMethod]
        public void UploadFilesToStorage()
        {
            
        }
    }
}
