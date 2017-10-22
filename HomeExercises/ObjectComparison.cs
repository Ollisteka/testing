using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class ObjectComparison
	{
		[Test]
		[Description("Проверка текущего царя")]
		[Category("ToRefactor")]
		public void CheckCurrentTsar()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();

			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

            // Перепишите код на использование Fluent Assertions.

            #region BeforeRefactor
            //			Assert.AreEqual(actualTsar.Name, expectedTsar.Name);
            //			Assert.AreEqual(actualTsar.Age, expectedTsar.Age);
            //			Assert.AreEqual(actualTsar.Height, expectedTsar.Height);
            //			Assert.AreEqual(actualTsar.Weight, expectedTsar.Weight);
            //
            //			Assert.AreEqual(expectedTsar.Parent.Name, actualTsar.Parent.Name);
            //			Assert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
            //			Assert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
            //			Assert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
		    #endregion

            expectedTsar.ShouldBeEquivalentTo(actualTsar, 
                options => options.Excluding(o => o.SelectedMemberPath.Contains("Id")));
        }

		[Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
			new Person("Vasili III of Russia", 28, 170, 60, null));

            // Какие недостатки у такого подхода? 
            /*
             
             1) Есть возможность уйти в рекурсию слишком глубоко и словить исключение. 
             2) При появлении новых свойств, которые мы также хотим учитывать при сравнении, необходимо дописывать новое условие.
             3) Всегда есть вероятность, что мы что-то забыли дописать.
             4) Немного непонятный синтаксис, хочется сразу писать Assert.Equals, раз уж мы сравниваем на равенство, а не Assert.True
             5) Мы не сможем узнать, где именно не сошлись данные, если возникнет ошибка.
             
            */
            Assert.True(AreEqual(actualTsar, expectedTsar));

		}

		private bool AreEqual(Person actual, Person expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
			actual.Name == expected.Name
			&& actual.Age == expected.Age
			&& actual.Height == expected.Height
			&& actual.Weight == expected.Weight
			&& AreEqual(actual.Parent, expected.Parent);
		}
	}

	public class TsarRegistry
	{
		public static Person GetCurrentTsar()
		{
			return new Person(
				"Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));
		}
	}

	public class Person
	{
		public static int IdCounter = 0;
		public int Age, Height, Weight;
		public string Name;
		public Person Parent;
		public int Id;

		public Person(string name, int age, int height, int weight, Person parent)
		{
			Id = IdCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}
