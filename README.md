# CSharp_3Sem

## Lab5
### Добавление асинхронного поведения
Все та же 4 лабораторная, но теперь с асинхронными методами.
#### Изменения
Рефакторинг кода для добавления асинхронного поведения. Рефакторинг касается всех элементов.
Переведены все операции ввода-вывода (IO operations) в асинхронный вариант.
Использован паттерн TAP (Task-based asynchronous pattern).
#### Немного об асинхронности
Асинхронность позволяет вынести отдельные задачи из основного потока в специальные асинхронные методы. При запросах к базе данных асинхронный метод уснет на время, пока не получит данные от БД, а основной поток сможет продолжить свою работу. В синхронном же приложении, если бы код получения данных находился в основном потоке, этот поток просто бы блокировался на время получения данных.
#### Что за TAP?
TAP состоит из типов: System.Threading.Tasks.Task и System.Threading.Tasks.Task в пространсте имен System.Threading.Tasks, которые используются для представления произвольных асинхронных операций.


# С Рождеством! Всего Вам наилучшего, теплого и доброго!
