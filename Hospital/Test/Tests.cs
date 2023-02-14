using AutoFixture;
using Hospital.DTO.Patient;
using Hospital.Helpers;
using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Services;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Test
{
    public class Tests
    {
        Mock<IExaminationRepository> _examinationRepository;
        Mock<IPatientRepository> _patientRepository;
        Mock<IVisitRepository> _visitRepository;
        Mock<IUserService> _userService;
        Mock<PatientService> _patientService;
        Mock<UserManager<User>> _userManager;
        Fixture _fixture;

        private List<string> correntStatuses = new List<string>();

        public Tests()
        {
            correntStatuses.Add("Zaplanowane");
            correntStatuses.Add("Zakoñczone");
            _patientRepository = new Mock<IPatientRepository>();
            _visitRepository = new Mock<IVisitRepository>();
            _examinationRepository = new Mock<IExaminationRepository>();
            _userService = new Mock<IUserService>();
            _userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _patientService = new Mock<PatientService>(_patientRepository.Object, _userService.Object);
            _fixture = new Fixture();

        }

        [Theory]
        [InlineData(Status.Planned)]
        [InlineData(Status.Finished)]
        public void TestMapper(Status status)
        {
            var result = Mapper.MapStatus(status);
            Assert.Contains(result, correntStatuses);
        }

        [Fact]
        public void TestToString()
        {
            var user = new User()
            {
                FirstName = "Jan",
                LastName = "Nowak"
            };

            var patient = new Patient()
            {
                Name = "Adam",
                LastName = "Kowalski"
            };

            Assert.Equal("Jan Nowak", user.ToString());
            Assert.Equal("Adam Kowalski", patient.ToString());            
        }

        [Fact]
        public void TestPatientMapping()
        {
            var user = _fixture.Build<User>().Without(x => x.Patients).Without(x => x.Visits).Create();
            _userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(user);
            IPatientService patientService = new PatientService(_patientRepository.Object, _userService.Object);

            var patient = _fixture.Build<Patient>().Without(x => x.Doctor).Without(x => x.Examinations).Create();
            patient.Examinations = _fixture.Build<Examination>().Without(x => x.Patient).CreateMany().ToList();
            
            var mappedPatient = patientService.MapPatient(patient);
            Assert.NotNull(mappedPatient);
            Assert.Equal(user.FirstName + " " + user.LastName, mappedPatient.DoctorFullName);
            Assert.NotNull(patient.Examinations);
        }

        [Fact]
        public void TestCreatePatient()
        {
            var dto = _fixture.Create<CreatePatientDto>();
            var doctorId = _fixture.Create<Guid>();
            IPatientService patientService = new PatientService(_patientRepository.Object, _userService.Object);
            patientService.CreatePatient(dto, doctorId);

            _patientRepository.Verify(x => x.CreatePatient(It.IsAny<Patient>()));
        }

        [Fact]
        public void TestDeleteVisit()
        {
            var visitId = _fixture.Create<Guid>();
            IVisitService visitService = new VisitService(_visitRepository.Object);
            visitService.DeleteVisit(visitId);
            _visitRepository.Verify(x => x.DeleteVisit(It.IsAny<Guid>()));
        }

        [Fact]
        public void TestGetExamination()
        {
            var examinationId = _fixture.Create<Guid>();
            _examinationRepository.Setup(x => x.GetExamination(It.IsAny<Guid>())).Returns(_fixture.Build<Examination>().Without(x => x.Patient).Create());
            IExaminationService examinationService = new ExaminationService(_examinationRepository.Object, _patientService.Object, _userManager.Object);
            var examination = examinationService.GetExamination(examinationId);
            Assert.NotNull(examination);
        }
    }
}