import { TechnikiInternetoweWebClientPage } from './app.po';

describe('techniki-internetowe-web-client App', function() {
  let page: TechnikiInternetoweWebClientPage;

  beforeEach(() => {
    page = new TechnikiInternetoweWebClientPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
