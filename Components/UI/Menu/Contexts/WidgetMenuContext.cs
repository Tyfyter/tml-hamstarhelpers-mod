﻿using HamstarHelpers.Services.Menus;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menus {
	public class WidgetMenuContext : MenuContext {
		public readonly bool IsInner;
		public readonly UIElement MyElement;



		////////////////

		public WidgetMenuContext( UIElement myElem, bool isInner ) {
			this.MyElement = myElem;
			this.IsInner = isInner;
		}

		public override void OnContexualize( string uiClassName, string contextName ) { }


		////////////////

		public override void Show( UIState ui ) {
			UIElement elem = this.GetInsertElem( ui );
			elem.Append( this.MyElement );
		}
		
		public override void Hide( UIState ui ) {
			this.MyElement.Remove();

			UIElement elem = this.GetInsertElem( ui );
			elem.RemoveChild( this.MyElement );
		}


		////////////////
		
		private UIElement GetInsertElem( UIState ui ) {
			if( this.IsInner ) {
				UIElement uiOuterContainer = MenuContextService.GetMenuContainerOuter( ui );
				UIElement uiInnerContainer = MenuContextService.GetMenuContainerInner( uiOuterContainer );

				return MenuContextService.GetMenuContainerInsertPoint( uiInnerContainer );
			} else {
				return ui;//uiOuterContainer;
			}
		}
	}
}