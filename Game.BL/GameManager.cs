using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.DAL.DataObject;
using Game.DAL.Repository;

namespace Game.BL
{
    public class GameManager
    {
        private static Random rand = new Random();
        public int guesses = 5;

        public static IPlayObject GenerateRandomWord(int typeOfWord)
        {
            if (typeOfWord == (int)TypeOfWord.Towns)
            {
                var towns = GetAllTowns();
                int townIndex = rand.Next(1, towns.Count() + 1);
                var town = GetTownById(townIndex);
                return town;
            }
            if (typeOfWord == (int)TypeOfWord.Cars)
            {
                var cars = GetAllCars();
                int carIndex = rand.Next(1, cars.Count() + 1);
                var car = GetCarById(carIndex);
                return car;
            }
            var animals = GetAllAnimals();
            int animalIndex = rand.Next(1, animals.Count() + 1);
            var animal = GetAnimalById(animalIndex);
            return animal;
        }

        private char[] ConvertModelNameToArray(object model)
        {
            if (model.GetType()==typeof(Town))
            {
                string name = ((Town) model).Name;

                char[] arrName = new char[name.Length];

                for (int i = 0; i < name.Length; i++)
                {
                    arrName[i] = name[i];
                }

                Word.PartialWord = new char[arrName.Length];

                return arrName;
            }
            else if (model.GetType() == typeof(Car))
            {
                string carModel = ((Car)model).Model;

                char[] arrModel = new char[carModel.Length];

                for (int i = 0; i < carModel.Length; i++)
                {
                    arrModel[i] = carModel[i];
                }

                Word.PartialWord = new char[arrModel.Length];

                return arrModel;
            }

            string animal = ((Animal)model).Breed;

            char[] arrBreed = new char[animal.Length];

            for (int i = 0; i < arrBreed.Length; i++)
            {
                arrBreed[i] = animal[i];
            }

            Word.PartialWord = new char[arrBreed.Length];

            return arrBreed;

        }

        public void SearchLetter(string letter)
        {
            bool isFound = false;
            char charLetter = char.Parse(letter);

            for (int k = 0; k < Word.FullWord.Length; k++)
            {
                if (char.ToLower(Word.FullWord[k]) == char.ToLower(charLetter))
                {
                    isFound = true;

                    if (k == 0)
                    {
                        Word.PartialWord[k] = charLetter;
                    }
                    else
                    {
                        Word.PartialWord[k] = char.ToLower(charLetter);
                    }
                }
            }

            for (int j = 0; j < Word.UsedLetters.Length; j++)
            {
                if (Word.UsedLetters[j] == char.ToUpper(charLetter))
                {
                    Word.UsedLetters[j] = char.ToLower(charLetter);
                }
            }

            if (isFound == false)
            {
                guesses = guesses - 1;
            }
        }

        public bool IfTheWordIsComplete()
        {
            return Word.PartialWord.All(t => char.IsLetter(t) != false);
        }

        public void GetStartSecretWord(int typeOfModel)
        {
            Word.UsedLetters = new char[] { 'A', 'B','C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            var secretWord = GenerateRandomWord(typeOfModel);
            Word.FullWord = ConvertModelNameToArray(secretWord);
        }

        private static IEnumerable<Town> GetAllTowns()
        {
            return TownRepository.GetAll();
        }

        private static IEnumerable<Car> GetAllCars()
        {
            return CarRepository.GetAll();
        }

        private static IEnumerable<Animal> GetAllAnimals()
        {
            return AnimalRepository.GetAll();
        }

        private static Town GetTownById(int id)
        {
            return TownRepository.GetById(id);
        }

        private static Car GetCarById(int id)
        {
            return CarRepository.GetById(id);
        }

        private static Animal GetAnimalById(int id)
        {
            return AnimalRepository.GetById(id);
        }
    }
}
