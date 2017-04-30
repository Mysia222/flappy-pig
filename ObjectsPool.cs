using System;
using System.Collections.Generic;

namespace Flappy
{
    /// <summary>
    /// Элемент пула объектов
    /// </summary>
    /// <typeparam name="T">Тип элемента пула</typeparam>
    internal struct PoolItem<T>
        where T : new()
    {
        /// <summary>
        /// Непосредственный элемент пула объектов
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Используется ли объект клиентом
        /// </summary>
        public bool Used { get; set; }

        // Создание
        public PoolItem(T item) : this()
        {
            this.Item = item;
            this.Used = false;
        }
    }

    /// <summary>
    /// Представляет пул повторно используемых объектов
    /// </summary>
    public class ObjectsPool<T>
        where T : new()
    {
        // Максимальное число объектов в пуле
        private const int MAX_OBJECTS = 10;

        // Экземпляр
        public static ObjectsPool<T> Instance { get; private set; }

        // Объекты пула и признак использования
        private PoolItem<T>[] objects;

        /// <summary>
        /// Запросить свободный объект из пула
        /// </summary>
        /// <returns>Свободный объект пула</returns>
        public T GetItem()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (!objects[i].Used)
                {
                    objects[i].Used = true;
                    return objects[i].Item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Освободить объект пула
        /// </summary>
        /// <param name="item">Освобождаемый объект</param>
        public void ReleaseItem(T item)
        {
            for(int i = 0; i < objects.Length; i++)
            {
                if(objects[i].Item.Equals(item))
                {
                    objects[i].Used = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Освободить все объекты пула
        /// </summary>
        public void ReleaseAll()
        {
            for (int i = 0; i < objects.Length; i++ )
            {
                objects[i].Used = false;
            }
        }

        /// <summary>
        /// Создание пула повторно используемых объектов
        /// </summary>
        public ObjectsPool(Action<T> initializer = null)
        {
            objects = new PoolItem<T>[MAX_OBJECTS];
            for(int i = 0; i < objects.Length; i++)
            {
                var newItem = new T();
                if (initializer != null)
                    initializer(newItem);
                objects[i] = new PoolItem<T>(newItem);
            }
        }
    }
}
