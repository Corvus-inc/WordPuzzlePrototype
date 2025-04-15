Общий обзор архитектуры:
Многослойная структура
Проект разделён на три основных слоя (и соответствующие им Assembly Definition):

Core: инфраструктурный/системный уровень.
Game: доменная (игровая) логика и правила.
UI: пользовательский интерфейс и всё визуальное.

Это даёт чёткую изоляцию обязанностей: UI не хранит логику, а Game не зависит от деталей Core.

Dependency Injection (DI) через Zenject:
Устраняет жёсткие зависимости и глобпльные синглтоны.
Каждый класс получает внешние сервисы (например, загрузку ресурсов, конфигурацию уровней, музыку) через конструктор или поля [Inject], что упрощает тестирование и поддержку.

MVx (MVP/MVC/MVVM) — в облегченном виде:
UI-скрипты можно считать View,
Game-классы (GameFlowController, LevelService) — контролирующая/модельная часть.
“Модель” (состояние игры) живёт в классе LevelService или других доменных сервисах.
“View” реагирует, вызывая методы сервисов, и отображает данные. Нет строгого MVP с Presenter’ом на каждый UI-элемент, но общий принцип разделения обязанностей соблюдён.

В проекте задействована система Zenject Signals (SignalBus) для реализации подхода «Event-Driven Architecture» между слоями:
Сигналы (Events) объявляются в Game-сборке (домен).

