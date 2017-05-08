using System;

namespace Flappy
{
    /// <summary>
    /// Игровое поле
    /// </summary>
    public interface IGameField
    {
        /// <summary>
        /// Положение земли по вертикали
        /// </summary>
        double GroundPosition { get; }

        /// <summary>
        /// Скорость движения
        /// </summary>
        double MoveSpeed { get; }

        /// <summary>
        /// Ширина поля
        /// </summary>
        double FieldWidth { get; }
    }
}
