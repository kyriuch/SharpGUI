using Autofac;
using iCreator.Elements;
using iCreator.External;
using iCreator.Models;
using iCreator.Providers;
using iCreator.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iCreator.Window
{
    internal interface IScene
    {
        void Load(View newView);
        void Unload();
        void OnUpdateFrame(FrameEventArgs e);
        void OnRenderFrame(FrameEventArgs e);
        void OnResize(EventArgs e);
        void OnKeyUp(KeyboardKeyEventArgs e);
        void OnKeyDown(KeyboardKeyEventArgs e);
        void OnMouseUp(MouseButtonEventArgs e);
        void OnMouseDown(MouseButtonEventArgs e);
        void OnMouseMove(MouseMoveEventArgs e);
    }

    internal sealed class Scene : IScene
    {
        public View view { get; private set; }

        private readonly ILogger logger;
        private readonly IColorHelper colorHelper;
        private readonly IVectorHelper vectorHelper;
        private readonly ApplicationWindow applicationWindow;

        private readonly List<Elements.Element> elements = new List<Elements.Element>();

        private Color4 backgroundColor;
        private Color4 primaryColor;
        private Color4 textColor;
        private Color4 tooltipColor;

        private IController controller;
        private List<FieldInfo> controllerFields;

        public Scene(ILogger logger, IColorHelper colorHelper, IVectorHelper vectorHelper, ApplicationWindow applicationWindow)
        {
            this.logger = logger;
            this.colorHelper = colorHelper;
            this.vectorHelper = vectorHelper;
            this.applicationWindow = applicationWindow;
        }

        public void Load(View newView)
        {
            applicationWindow.Title = newView.Name;
            applicationWindow.Width = newView.Width;
            applicationWindow.Height = newView.Height;

            setupController(newView);

            backgroundColor = colorHelper.ToTKColor(newView.Theme.BackgroundColor);
            primaryColor = colorHelper.ToTKColor(newView.Theme.PrimaryColor);
            textColor = colorHelper.ToTKColor(newView.Theme.TextColor);
            tooltipColor = colorHelper.ToTKColor(newView.Theme.TooltipColor);

            newView.Elements.ForEach(element =>
            {
                if (string.IsNullOrEmpty(element.Name))
                {
                    logger.Warning("Element without a name ignored.");
                }
                else switch (element.Type)
                    {
                        case "Label": setupLabel(element); break;
                        case "TextField": setupTextField(element); break;
                        case "Button": setupButton(element); break;
                        case "Image": setupImage(element); break;
                    }
            });

            controller.Initialize();
        }

        public void Unload()
        {

        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            GL.Viewport(applicationWindow.ClientRectangle);

            elements.ForEach(element => element.OnUpdateFrame(e));
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            elements.Where(element => element.IsVisible).ToList().ForEach(element => element.OnRenderFrame(e));
        }

        public void OnResize(EventArgs e)
        {
            elements.ForEach(element => element.OnResize(e));
        }

        public void OnKeyUp(KeyboardKeyEventArgs e)
        {
            elements.ForEach(element => element.OnKeyUp(e));
        }

        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            elements.ForEach(element => element.OnKeyDown(e));
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            elements.ForEach(element => element.OnMouseUp(e));
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            elements.ForEach(element => element.OnMouseDown(e));
        }

        public void OnMouseMove(MouseMoveEventArgs e)
        {
            elements.ForEach(element => element.OnMouseMove(e));
        }

        private void setupLabel(Models.Element element)
        {
            Label label = ContainerProvider.Scope.Resolve<Label>();

            Color4 textColor = string.IsNullOrEmpty(element.TextColor) ? this.textColor :
                colorHelper.ToTKColor(element.TextColor);

            label.Setup(vectorHelper.ToTKVector2(element.Position),
                element.Text, textColor, element.FontSize);

            addReferenceToAController(element, label);

            elements.Add(label);
        }

        private void setupTextField(Models.Element element)
        {
            TextField textField = ContainerProvider.Scope.Resolve<TextField>();

            TextFieldType type = TextFieldType.RoundedRectangle;

            if (element.ShapeType == 1)
                type = TextFieldType.Rectangle;

            TextType textType = TextType.Text;

            if (element.TextType == 1)
                textType = TextType.Password;

            Color4 primaryColor = string.IsNullOrEmpty(element.Color) ? this.primaryColor : colorHelper.ToTKColor(element.Color);
            Color4 textColor = string.IsNullOrEmpty(element.TextColor) ? this.textColor : colorHelper.ToTKColor(element.TextColor);
            Color4 tooltipColor = string.IsNullOrEmpty(element.TooltipColor) ? this.tooltipColor : colorHelper.ToTKColor(element.TooltipColor);

            textField.Setup(vectorHelper.ToTKVector2(element.Position), vectorHelper.ToTKVector2(element.Size),
                primaryColor, textColor, element.Tooltip, tooltipColor, element.FontSize, type, textType);

            addReferenceToAController(element, textField);

            elements.Add(textField);
        }

        private void setupButton(Models.Element element)
        {
            if (string.IsNullOrEmpty(element.Text))
            {
                logger.Warning($"Button { element.Name } has no text provided. Ignoring element.");

                return;
            }

            Button button = ContainerProvider.Scope.Resolve<Button>();

            ButtonType type = ButtonType.RoundedRectangleNoFill;

            switch (element.ShapeType)
            {
                case 1: type = ButtonType.RectangleNoFill; break;
                case 2: type = ButtonType.RoundedRectangleFill; break;
                case 3: type = ButtonType.RectangleFill; break;
            }

            Color4 primaryColor = string.IsNullOrEmpty(element.Color) ? this.primaryColor : colorHelper.ToTKColor(element.Color);
            Color4 textColor = string.IsNullOrEmpty(element.TextColor) ? this.textColor : colorHelper.ToTKColor(element.TextColor);

            button.Setup(vectorHelper.ToTKVector2(element.Position), element.Text,
                vectorHelper.ToTKVector2(element.Size), primaryColor, textColor, element.FontSize, type);

            addReferenceToAController(element, button);

            elements.Add(button);
        }

        private void setupImage(Models.Element element)
        {
            if (string.IsNullOrEmpty(element.FileName))
            {
                logger.Warning($"Image { element.Name } has no filename provided. Ignoring element.");

                return;
            }

            if (element.Scale <= 0)
            {
                logger.Warning($"Image { element.Name } has to have scale higher than 0. Ignoring element.");

                return;
            }

            Image image = ContainerProvider.Scope.Resolve<Image>();

            Color4 color = string.IsNullOrEmpty(element.Color) ? Color4.White : colorHelper.ToTKColor(element.Color);

            image.Setup($"iCreator\\Resources\\{ element.FileName }",
                vectorHelper.ToTKVector2(element.Position), color, element.Scale);

            addReferenceToAController(element, image);

            elements.Add(image);
        }

        private void setupController(View view)
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();

            string nameSpace = entryAssembly.GetTypes().FirstOrDefault().Namespace;

            Type type = entryAssembly.GetType($"{ nameSpace }.iCreator.Controllers.{ view.Filename }Controller");

            controller = (IController) Activator.CreateInstance(type);

            controllerFields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToList();

            if(controllerFields.Any(x => x.FieldType.Name.Equals("ILogger")))
            {
                FieldInfo field = controllerFields.FirstOrDefault(x => x.FieldType.Name.Equals("ILogger"));

                field.SetValue(controller, logger);
            }
        }

        private void addReferenceToAController(Models.Element modelElement, Elements.Element element)
        {
            if (controllerFields.Any(x => x.Name.Equals(modelElement.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                FieldInfo field = controllerFields.FirstOrDefault(x => x.Name.Equals(modelElement.Name, StringComparison.CurrentCultureIgnoreCase));

                if (field.FieldType.Name.Equals(modelElement.Type))
                    field.SetValue(controller, element);
                else
                    logger.Warning($"Field { field.Name } should be a type of { modelElement.Type }.");
            }
        }
    }
}
