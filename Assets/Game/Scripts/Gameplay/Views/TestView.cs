using AutoGenerateUI.Test;

namespace Game.View {
    public class TestView : BaseView {


        private TestUI m_ui;
        private void Start() {
            m_ui = new TestUI(transform);

        }

    }
}