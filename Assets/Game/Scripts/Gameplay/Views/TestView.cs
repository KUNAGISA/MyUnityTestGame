using AutoGenerateUI.Test;

namespace Game.View {
    public class TestView : BaseView {


        private TestUI m_ui;
        private void Awake() {
            m_ui = new TestUI(transform);

        }

    }
}