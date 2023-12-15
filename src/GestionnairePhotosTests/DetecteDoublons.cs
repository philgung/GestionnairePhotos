using GestionnairePhotosConsole;

namespace GestionnairePhotosTests;

public class DetecteDoublons
{
    [Test]
    public void A_un_doublon()
    {
        var aUnDoublon = Detecteur.AUnDoublon(Original, Doublon);

        aUnDoublon.Should().BeTrue();
    }

    [Test]
    public void Na_pas_de_doublons()
    {
        var aUnDoublon = Detecteur.AUnDoublon(Original, AutreImage);

        aUnDoublon.Should().BeFalse();
    }

    private const string Original = "Ressources/Flag_of_France.svg.png";
    private const string Doublon = "Ressources/Flag_of_France.svg-Copie.png";
    private const string AutreImage = "Ressources/Flag_of_the_United_States.svg.png";
}