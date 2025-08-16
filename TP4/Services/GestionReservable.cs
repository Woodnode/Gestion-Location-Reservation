using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using TP4.Models;

namespace TP4.Services
{
    public class GestionReservable
    {
        private static readonly List<Voiture> ListeVoitures = [];
        private static readonly List<Chambre> ListeChambres = [];
        public static readonly List<Reservation> ListeReservations = [];
        private static readonly List<string> ListeDeMarques = ["Kia", "Ford", "Mazda", "Toyota", "Hyundai", "Honda"];

        public static int GenererId(string type) 
        {
            if (type != "Reservation")
            {
                int id = 0;
                string cheminFichier = @".\monfichier" + type + ".txt";
                var lignes = File.ReadAllLines(cheminFichier);

                if (lignes.Length > 0)
                {
                    var derniereLigne = lignes[^1];
                    var champs = derniereLigne.Split(';');
                    id = int.Parse(champs[0]);
                }
                return id + 1;
            }
            else
            {
                Random random = new();
                int id = random.Next(1, 10000);

                while (ListeReservations.Any(reservation => reservation.Id == id))
                {
                    id = random.Next(1, 10000);
                }
                return id;
            }
        }

        protected static void SauvegarderListeReservable(string type)
        {
            string cheminFichier = @".\monfichier" + type + ".txt";
            List<IReservable> listeASauvegarder = ObtenirListeReservable(type);
            using StreamWriter sw = new(cheminFichier);

            if (type == "Reservation")
            {
                foreach (var reservation in ListeReservations)
                {
                    sw.WriteLine($"{reservation.Id};{reservation.DateDebut};{reservation.DateFin};" +
                        $"{reservation.ObjetDeLaReservation.GetType().Name};{reservation.ObjetDeLaReservation.Id};" +
                        $"{reservation.Prix}");
                }
            }
            else
            {
                foreach (var reservable in listeASauvegarder)
                {
                    string ligne = string.Join(";", reservable.GetType().GetProperties().Select(prop => prop.GetValue(reservable)));
                    sw.WriteLine(ligne);
                }
            }
        }

        public static void ChargerListeDepuisFichier()
        {
            Dictionary<string, Type> listeTypes = new()
            {
                { "Voiture", typeof(Voiture) },
                { "Chambre", typeof(Chambre) },
                { "Reservation", typeof(Reservation) }
            };

            ListeChambres.Clear();
            ListeVoitures.Clear();
            ListeReservations.Clear();

            foreach (var type in listeTypes)
            {
                string cheminFichier = @".\monfichier" + type.Key + ".txt";

                if (!File.Exists(cheminFichier)) File.Create(cheminFichier).Close();
                else
                {
                    var lignes = File.ReadAllLines(cheminFichier);

                    foreach (var ligne in lignes)
                    {
                        var champs = ligne.Split(';');
                        var reservable = Activator.CreateInstance(type.Value) ?? throw new InvalidOperationException("Erreur lors de la création de l'objet reservable");
                        PropertyInfo[] props = reservable.GetType().GetProperties();

                        for (int i = 0; i < props.Length; i++)
                        {
                            if (props[i].PropertyType == typeof(int)) props[i].SetValue(reservable, int.Parse(champs[i]));
                            else if (props[i].PropertyType == typeof(string)) props[i].SetValue(reservable, champs[i]);
                            else if (props[i].PropertyType == typeof(DateTime)) props[i].SetValue(reservable, DateTime.Parse(champs[i]));
                            else if (props[i].Name == "ObjetDeLaReservation")
                            {
                                IReservable? objetDeLaReservation = ObtenirReservableParId(champs[i], int.Parse(champs[i + 1]));
                                props[i].SetValue(reservable, objetDeLaReservation);
                                props[i + 1].SetValue(reservable, int.Parse(champs[i + 2]));
                                i += 2;
                            }
                        }

                        if (type.Key == "Voiture") ListeVoitures.Add((Voiture)reservable);
                        else if (type.Key == "Chambre") ListeChambres.Add((Chambre)reservable);
                        else if (type.Key == "Reservation") ListeReservations.Add((Reservation)reservable);
                    }
                }
            }
        }

        public static List<IReservable> ObtenirListeReservable(string type)
        {
            return type switch
            {
                "Voiture" => [..ListeVoitures.Cast<IReservable>()],
                "Chambre" => [.. ListeChambres.Cast<IReservable>()],
                "Reservation" => [.. ListeReservations.Cast<IReservable>()],
                _ => throw new ArgumentException("Type de liste inconnu"),
            };
        }

        public static List<string> ObtenirListeDeMarques() => ListeDeMarques;

        public static void AjouterReservable(IReservable reservable)
        {
            if (reservable is Voiture voiture) ListeVoitures.Add(voiture);
            else if (reservable is Chambre chambre) ListeChambres.Add(chambre);
            else if (reservable is Reservation reservation) ListeReservations.Add(reservation);
            else throw new ArgumentException("Type de réservation inconnu");

            SauvegarderListeReservable(reservable.GetType().Name);
        }

        public static void SupprimerReservable(IReservable reservable)
        {
            IReservable? reservableASupprimer = ObtenirListeReservable(reservable.GetType().Name).SingleOrDefault(r => r.Id == reservable.Id);

            if (reservableASupprimer is Voiture voiture) ListeVoitures.Remove(voiture);
            else if (reservableASupprimer is Chambre chambre) ListeChambres.Remove(chambre);
            else if (reservableASupprimer is Reservation reservation) ListeReservations.Remove(reservation);
            else throw new ArgumentException("Type de réservation inconnu");

            SauvegarderListeReservable(reservableASupprimer.GetType().Name);
        }

        public static IReservable? ObtenirReservableParId(string type, int id) => ObtenirListeReservable(type).SingleOrDefault(l => l.Id == id);

        public static void ModifierReservable(IReservable reservable)
        {
            IReservable reservableAModifier = ObtenirListeReservable(reservable.GetType().Name).Single(r => r.Id == reservable.Id);
            
            PropertyInfo[] props = reservable.GetType().GetProperties();
            foreach (var prop in props)
            {
                reservableAModifier.GetType().GetProperty(prop.Name)?.SetValue(reservableAModifier, prop.GetValue(reservable));
            }

            SauvegarderListeReservable(reservable.GetType().Name);
        }

        public static List<Reservation> TrierListeReservation(string tri, string type)
        {
            List<Reservation> listeReservationTriee = [.. ListeReservations.Where(r => r.ObjetDeLaReservation.GetType().Name == type)];

            if (tri == "DateDebut") listeReservationTriee.Sort((x, y) => y.DateDebut.CompareTo(x.DateDebut));
            else if (tri == "DateFin") listeReservationTriee.Sort((x, y) => y.DateFin.CompareTo(x.DateFin));
            else if (tri == "Prix") listeReservationTriee.Sort((x, y) => y.Prix.CompareTo(x.Prix));

            return listeReservationTriee;
        }

        public static bool EstReserve(int id, string type)
        {
            if (ObtenirListeReservable("Reservation").Cast<Reservation>().ToList().Where(r => r.ObjetDeLaReservation.GetType().Name == type)
              .Any(r => r.ObjetDeLaReservation.Id == id)) return true;

            return false;
        }
    }
}