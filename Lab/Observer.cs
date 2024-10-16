using System;
using System.Collections.Generic;

public interface IObserver
{
    void Update(float temperature);
}

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

public class WeatherStation : ISubject
{
    private List<IObserver> observers;
    private float temperature;

    public WeatherStation()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if (!observers.Remove(observer))
        {
            Console.WriteLine("Наблюдатель не найден.");
        }
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature);
        }
    }

    public void SetTemperature(float newTemperature)
    {
        if (newTemperature < -100 || newTemperature > 100)
        {
            throw new ArgumentOutOfRangeException("Температура должна быть в пределах -100°C до 100°C.");
        }

        Console.WriteLine($"Изменение температуры: {newTemperature}°C");
        temperature = newTemperature;
        NotifyObservers();
    }
}

public class WeatherDisplay : IObserver
{
    private string _name;

    public WeatherDisplay(string name)
    {
        _name = name;
    }

    public void Update(float temperature)
    {
        Console.WriteLine($"{_name} показывает новую температуру: {temperature}°C");
    }
}


public class SoundAlert : IObserver
{
    public void Update(float temperature)
    {
        Console.WriteLine($"Звуковое уведомление: температура изменилась до {temperature}°C");
    }
}

class Program
{
    static void Main(string[] args)
    {
        WeatherStation weatherStation = new WeatherStation();

        
        WeatherDisplay mobileApp = new WeatherDisplay("Мобильное приложение");
        WeatherDisplay digitalBillboard = new WeatherDisplay("Электронное табло");
        SoundAlert soundAlert = new SoundAlert();

        
        weatherStation.RegisterObserver(mobileApp);
        weatherStation.RegisterObserver(digitalBillboard);
        weatherStation.RegisterObserver(soundAlert);

        
        weatherStation.SetTemperature(25.0f);
        weatherStation.SetTemperature(30.0f);

        
        weatherStation.RemoveObserver(digitalBillboard);
        weatherStation.SetTemperature(28.0f);

        
        weatherStation.RemoveObserver(digitalBillboard);

        
        try
        {
            weatherStation.SetTemperature(150.0f);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
