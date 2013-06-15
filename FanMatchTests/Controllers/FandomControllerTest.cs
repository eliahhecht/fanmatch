using FanMatch.Controllers;
using FanMatch.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FanMatchTests.Controllers
{
    public class FandomControllerTest
    {
        private Mock<IFandomRepository> repo;
        private FandomController contr;

        public FandomControllerTest()
        {
            this.repo = new Mock<IFandomRepository>();
            this.contr = new FandomController(() => repo.Object);
        }

        [Fact]
        public void DuplicativelyNamedFandomsCannotBeCreated()
        {
            const string name = "haaaaary potter";
            this.repo.Setup(r => r.GetByName(name))
                .Returns(new Fandom());

            this.contr.Create(new Fandom { Name = name });

            this.repo.Verify(r => r.Create(It.IsAny<Fandom>()), Times.Never(),
                "This fandom already exists so it shouldn't be created");
        }

    }
}
