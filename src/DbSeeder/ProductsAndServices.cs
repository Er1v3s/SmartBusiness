using SalesService.Domain.Entities;

namespace DbSeeder
{
    public static class ProductsAndServices
    {
        public static List<Service> GenerateServices(string companyId) => new()
        {
            new Service
            {
                CompanyId = companyId,
                Name = "Makijaż okolicznościowy",
                Description = "Profesjonalny makijaż okolicznościowy.",
                Category = "Makijaż",
                Duration = 90,
                Price = 200,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Regulacja brwi",
                Description = "Precyzyjna regulacja brwi.",
                Category = "Brwi",
                Duration = 20,
                Price = 30,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Henna rzęs",
                Description = "Naturalna henna rzęs.",
                Category = "Rzęsy",
                Duration = 30,
                Price = 30,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Stylizacja brwi henną/farbką + geometria",
                Description =
                    "Zabieg łączący farbowanie brwi henną lub farbką z regulacją.",
                Category = "Brwi",
                Duration = 30,
                Price = 80,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Laminacja brwi",
                Description =
                    "Innowacyjna laminacja, nadająca brwiom wyrazisty i schludny kształt.",
                Category = "Brwi",
                Duration = 60,
                Price = 150,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Lifting rzęs z laminacją",
                Description =
                    "Zabieg liftingu rzęs z laminacją.",
                Category = "Rzęsy",
                Duration = 60,
                Price = 150,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Oczyszczanie manulane skóry",
                Description = "Ręczne oczyszczanie skóry twarzy odświeżające cerę.",
                Category = "Pielęgnacja skóry",
                Duration = 60,
                Price = 180,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Peeling kawitacyjny",
                Description = "Nowoczesny zabieg kawitacyjny, dogłębnie oczyszcza i odświeża skórę twarzy.",
                Category = "Pielęgnacja skóry",
                Duration = 30,
                Price = 170,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Peeling węglowy",
                Description =
                    "Laserowy peeling węglowy, oczyszcza skórę i poprawia jej koloryt.",
                Category = "Pielęgnacja skóry",
                Duration = 30,
                Price = 220,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Dermapen",
                Description =
                    "Mikronakłucia stymulują produkcję kolagenu i poprawiają kondycję skóry.",
                Category = "Medycyna estetyczna",
                Duration = 30,
                Price = 310,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Mezoterapia igłowa",
                Description =
                    "Iniekcje aktywnych koktajli rewitalizują skórę.",
                Category = "Medycyna estetyczna",
                Duration = 60,
                Price = 350,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Lipoliza",
                Description =
                    "Iniekcyjny zabieg modelowania sylwetki.",
                Category = "Medycyna estetyczna",
                Duration = 30,
                Price = 240,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Makijaż permanentny brwi",
                Description = "Trwały makijaż brwi.",
                Category = "Makijaż permanentny",
                Duration = 180,
                Price = 800,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Makijaż permanentny ust",
                Description =
                    "Permanentny makijaż ust.",
                Category = "Makijaż permanentny",
                Duration = 180,
                Price = 900,
                Tax = 8
            },
            new Service
            {
                CompanyId = companyId,
                Name = "Modelowanie ust",
                Description = "Tymczasowe modelowanie ust kwasem hialuronowym.",
                Category = "Medycyna estetyczna",
                Duration = 45,
                Price = 650,
                Tax = 8
            }
        };

        static List<Product> GenerateProducts(string companyId) => new()
        {
            new Product { CompanyId = companyId, Name = "Lakier hybrydowy", Description = "Trwały lakier do paznokci", Category = "Paznokcie", Price = 40, Tax = 8},
            new Product { CompanyId = companyId, Name = "Pilnik do paznokci", Description = "Profesjonalny pilnik do stylizacji", Category = "Paznokcie", Price = 15, Tax = 8 },
            new Product { CompanyId = companyId, Name = "Odżywka do paznokci", Description = "Wzmacniająca odżywka do paznokci", Category = "Paznokcie", Price = 50, Tax = 8 },
            new Product { CompanyId = companyId, Name = "Serum do twarzy", Description = "Intensywne serum nawilżające", Category = "Twarz", Price = 120, Tax = 8 },
            new Product { CompanyId = companyId, Name = "Krem pod oczy", Description = "Redukuje cienie i obrzęki", Category = "Twarz", Price = 90, Tax = 8 },
            new Product { CompanyId = companyId, Name = "Maseczka nawilżająca", Description = "Głęboko nawilżająca maseczka", Category = "Twarz", Price = 80, Tax = 8 },
            new Product { CompanyId = companyId, Name = "Pędzel do makijażu", Description = "Profesjonalny pędzel do aplikacji", Category = "Makijaż", Price = 60, Tax = 8 },
            new Product { CompanyId = companyId, Name = "Podkład do twarzy", Description = "Kryjący podkład o lekkiej formule", Category = "Makijaż", Price = 140, Tax = 8 },
        };
    }
}
