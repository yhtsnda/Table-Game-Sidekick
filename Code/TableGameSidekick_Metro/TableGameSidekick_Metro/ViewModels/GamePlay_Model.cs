﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using TableGameSidekick_Metro.DataEntity;
using System.Runtime.Serialization;
using MVVMSidekick.Reactive;
using MVVMSidekick.Commands;
using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.Storages;
namespace TableGameSidekick_Metro.ViewModels
{
    [DataContract]
    public class GamePlay_Model : ViewModelBase<GamePlay_Model>
    {
        protected IStorage<GamePlay_Model> m_StorageFactory;


        public GamePlay_Model()
        {
     

        }

        protected virtual async Task ConfigCommands()
        {
            #region SaveDataCommand 设置
            GetValueContainer(x => x.CurrentGameInfomation)
                .GetValueChangeObservable()
                .Select(x => x.EventArgs != null)
                .Subscribe(SaveDataCommand.CommandCore.CanExecuteObserver);

            SaveDataCommand.CommandCore.Subscribe
                (

                    async e =>
                    {
                        m_StorageFactory.Value = this;
                        await m_StorageFactory.Save();

                    }

                );


            #endregion


            await Task.Yield();
        }


        public Guid Id
        {
            get
            {
                return CurrentGameInfomation == null ? new Guid() : CurrentGameInfomation.Id;
            }
        }

    
        public GameInfomation CurrentGameInfomation
        {
            get { return m_CurrentGameInfomationLocator(this).Value; }
            set { m_CurrentGameInfomationLocator(this).SetValueAndTryNotify(value); }
        }


        #region Property GameInfomation CurrentGameInfomation Setup

        protected Property<GameInfomation> m_CurrentGameInfomation =
          new Property<GameInfomation> { LocatorFunc = m_CurrentGameInfomationLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<GameInfomation>> m_CurrentGameInfomationLocator =
            RegisterContainerLocator<GameInfomation>(
            "CurrentGameInfomation",
            model =>
            {
                model.m_CurrentGameInfomation =
                    model.m_CurrentGameInfomation
                    ??
                    new Property<GameInfomation> { LocatorFunc = m_CurrentGameInfomationLocator };
                return model.m_CurrentGameInfomation.Container =
                    model.m_CurrentGameInfomation.Container
                    ??
                    new ValueContainer<GameInfomation>("CurrentGameInfomation", model);
            });

        #endregion



    
        public BindableBase GameData
        {
            get { return m_GameDataLocator(this).Value; }
            set { m_GameDataLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property ViewModelBase GameData Setup
        protected Property<BindableBase> m_GameData =
          new Property<BindableBase> { LocatorFunc = m_GameDataLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<BindableBase>> m_GameDataLocator =
            RegisterContainerLocator<BindableBase>(
                "GameData",
                model =>
                {
                    model.m_GameData =
                        model.m_GameData
                        ??
                        new Property<BindableBase> { LocatorFunc = m_GameDataLocator };
                    return model.m_GameData.Container =
                        model.m_GameData.Container
                        ??
                        new ValueContainer<BindableBase>("GameData", model);
                });
        #endregion



        public CommandModel<ReactiveCommand, String> SaveDataCommand
        {
            get { return m_SaveDataCommand.WithViewModel(this); }
            protected set { m_SaveDataCommand = value; }
        }

        #region SaveDataCommand Configuration
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        CommandModel<ReactiveCommand, String> m_SaveDataCommand
            = new ReactiveCommand(canExecute: true).CreateCommandModel(default(String));
        #endregion














    }
}
